using System;
using System.Runtime.InteropServices;
using OsrStudio.Audio;
using OsrStudio.Native;
using OsrStudio.Video;
using OsrStudio.Windows.DirectX;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.MediaFoundation;
using Device = SharpDX.Direct3D11.Device;

namespace OsrStudio.Windows.MediaFoundation
{
    public class MfWriter : IVideoFileWriter
    {
        readonly Device _device;
        // Reduced from 8 Mbps to 3 Mbps for better encoder performance
        // 3 Mbps is more than sufficient for screen recording quality
        const int BitRate = 3_000_000;
        readonly Guid _encodingFormat;
        readonly Guid _encodedAudioFormat = AudioFormatGuids.Aac;
        readonly long _frameDuration;
        readonly SinkWriter _writer;

        static readonly MediaAttributeKey<RateControlMode> RateControlModeKey = new MediaAttributeKey<RateControlMode>("1c0608e9-370c-4710-8a58-cb6181c42423");
        static readonly MediaAttributeKey<int> QualityKey = new MediaAttributeKey<int>("fcbf57a3-7ea5-4b0c-9644-69b40c39c391");

        const int VideoStreamIndex = 0;
        const int AudioStreamIndex = 1;

        const int TenPower7 = 10_000_000;
        readonly int _bufferSize;
        readonly long _audioInBytesPerSecond;

        long _frameNumber = -1;

        // Keep this separate. First frame might be a RepeatFrame.
        bool _first = true;

        public static long PackLong(int Left, int Right)
        {
            return new PackedLong
            {
                Low = Right,
                High = Left
            }.Long;
        }

        MediaAttributes GetSinkWriterAttributes(Device Device)
        {
            var attr = new MediaAttributes(6);

            attr.Set(SinkWriterAttributeKeys.ReadwriteEnableHardwareTransforms, 1);
            attr.Set(SinkWriterAttributeKeys.ReadwriteDisableConverters, 0);
            attr.Set(TranscodeAttributeKeys.TranscodeContainertype, TranscodeContainerTypeGuids.Mpeg4);

            // REMOVED: LowLatency mode - it reduces buffering which actually hurts performance
            // The encoder needs some buffering to maintain throughput
            // attr.Set(SinkWriterAttributeKeys.LowLatency, true);

            // Disable throttling for better real-time performance
            attr.Set(SinkWriterAttributeKeys.ReadwriteDisableThrottling, 1);

            var devMan = new DXGIDeviceManager();
            devMan.ResetDevice(Device);
            attr.Set(SinkWriterAttributeKeys.D3DManager, devMan);

            return attr;
        }

        public MfWriter(VideoWriterArgs Args, Device Device, Guid EncodingFormat = default)
        {
            var inputFormat = Args.ImageProvider.DummyFrame is Texture2DFrame
                ? VideoFormatGuids.NV12
                : VideoFormatGuids.Rgb32;

            _device = Device;
            _encodingFormat = EncodingFormat == default ? VideoFormatGuids.H264 : EncodingFormat;

            _frameDuration = TenPower7 / Args.FrameRate;

            var attr = GetSinkWriterAttributes(Device);

            _writer = MediaFactory.CreateSinkWriterFromURL(Args.FileName, null, attr);

            var w = Args.ImageProvider.Width;
            var h = Args.ImageProvider.Height;
            _bufferSize = w * h * 4;

            using (var mediaTypeOut = new MediaType())
            {
                mediaTypeOut.Set(MediaTypeAttributeKeys.MajorType, MediaTypeGuids.Video);
                mediaTypeOut.Set(MediaTypeAttributeKeys.Subtype, _encodingFormat);
                mediaTypeOut.Set(MediaTypeAttributeKeys.AvgBitrate, BitRate);
                mediaTypeOut.Set(MediaTypeAttributeKeys.InterlaceMode, (int)VideoInterlaceMode.Progressive);
                mediaTypeOut.Set(MediaTypeAttributeKeys.FrameSize, PackLong(w, h));
                mediaTypeOut.Set(MediaTypeAttributeKeys.FrameRate, PackLong(Args.FrameRate, 1));
                mediaTypeOut.Set(MediaTypeAttributeKeys.PixelAspectRatio, PackLong(1, 1));
                _writer.AddStream(mediaTypeOut, out _);
            }

            using (var mediaTypeIn = new MediaType())
            {
                mediaTypeIn.Set(MediaTypeAttributeKeys.MajorType, MediaTypeGuids.Video);
                mediaTypeIn.Set(MediaTypeAttributeKeys.Subtype, inputFormat);
                mediaTypeIn.Set(MediaTypeAttributeKeys.InterlaceMode, (int)VideoInterlaceMode.Progressive);
                mediaTypeIn.Set(MediaTypeAttributeKeys.FrameSize, PackLong(w, h));
                mediaTypeIn.Set(MediaTypeAttributeKeys.FrameRate, PackLong(Args.FrameRate, 1));
                mediaTypeIn.Set(MediaTypeAttributeKeys.PixelAspectRatio, PackLong(1, 1));
                // REMOVED: AllSamplesIndependent = 1
                // Setting all frames as keyframes is extremely expensive for encoding
                // Let the encoder use P-frames and B-frames for much better performance

                var encoderParams = new MediaAttributes(2);
                encoderParams.Set(RateControlModeKey, RateControlMode.Quality);
                encoderParams.Set(QualityKey, Args.VideoQuality);
                _writer.SetInputMediaType(VideoStreamIndex, mediaTypeIn, encoderParams);
            }

            if (Args.AudioProvider != null)
            {
                var wf = Args.AudioProvider.WaveFormat;
                _audioInBytesPerSecond = wf.SampleRate * wf.Channels * wf.BitsPerSample / 8;

                using (var audioTypeOut = GetMediaType(wf))
                {
                    audioTypeOut.Set(MediaTypeAttributeKeys.Subtype, _encodedAudioFormat);
                    audioTypeOut.Set(MediaTypeAttributeKeys.AudioAvgBytesPerSecond, GetAacBitrate(Args.AudioQuality));
                    _writer.AddStream(audioTypeOut, out _);
                }

                using var audioTypeIn = GetMediaType(wf);
                audioTypeIn.Set(MediaTypeAttributeKeys.Subtype, AudioFormatGuids.Pcm);
                _writer.SetInputMediaType(AudioStreamIndex, audioTypeIn, null);
            }

            _writer.BeginWriting();

            // Create ring buffer of textures and samples for better throughput
            var textureDesc = new Texture2DDescription
            {
                CpuAccessFlags = CpuAccessFlags.Read,
                BindFlags = BindFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                Width = w,
                Height = h,
                OptionFlags = ResourceOptionFlags.None,
                MipLevels = 1,
                ArraySize = 1,
                SampleDescription = { Count = 1, Quality = 0 },
                Usage = ResourceUsage.Staging
            };

            for (int i = 0; i < BufferCount; i++)
            {
                _copyTextures[i] = new Texture2D(Device, textureDesc);
                _samples[i] = MediaFactory.CreateVideoSampleFromSurface(null);

                // Create the media buffer from the texture
                MediaFactory.CreateDXGISurfaceBuffer(typeof(Texture2D).GUID, _copyTextures[i], 0, false, out _mediaBuffers[i]);

                using (var buffer2D = _mediaBuffers[i].QueryInterface<Buffer2D>())
                    _mediaBuffers[i].CurrentLength = buffer2D.ContiguousLength;

                // Attach the created buffer to the sample
                _samples[i].AddBuffer(_mediaBuffers[i]);
            }
        }

        public static MediaType GetMediaType(WaveFormat Wf)
        {
            var mediaType = new MediaType();
            
            mediaType.Set(MediaTypeAttributeKeys.MajorType, MediaTypeGuids.Audio);
            mediaType.Set(MediaTypeAttributeKeys.AudioNumChannels, Wf.Channels);
            mediaType.Set(MediaTypeAttributeKeys.AudioBitsPerSample, Wf.BitsPerSample);
            mediaType.Set(MediaTypeAttributeKeys.AudioSamplesPerSecond, Wf.SampleRate);

            return mediaType;
        }

        public static int GetAacBitrate(int AudioQuality)
        {
            var i = (AudioQuality - 1) / 25;

            return new[]
            {
                12_000,
                16_000,
                20_000,
                24_000
            }[i];
        }

        readonly object _syncLock = new object();

        // Ring buffer for textures and samples to allow queuing multiple frames
        const int BufferCount = 3;
        readonly Texture2D[] _copyTextures = new Texture2D[BufferCount];
        readonly Sample[] _samples = new Sample[BufferCount];
        readonly MediaBuffer[] _mediaBuffers = new MediaBuffer[BufferCount];
        int _currentBufferIndex = 0;

        public void Write(Sample Sample)
        {
            lock (_syncLock)
            {
                if (_disposed)
                    return;

                Sample.SampleTime = _frameNumber * _frameDuration;
                Sample.SampleDuration = _frameDuration;

                if (_first)
                {
                    _writer.SendStreamTick(VideoStreamIndex, Sample.SampleTime);
                    Sample.Set(SampleAttributeKeys.Discontinuity, true);
                    _first = false;
                }
                _writer.WriteSample(VideoStreamIndex, Sample);
            }
        }

        void Write(Texture2D Texture)
        {
            // Use ring buffer to allow queuing multiple frames
            var bufferIndex = _currentBufferIndex;
            _currentBufferIndex = (_currentBufferIndex + 1) % BufferCount;

            _device.ImmediateContext.CopyResource(Texture, _copyTextures[bufferIndex]);

            Write(_samples[bufferIndex]);
        }

        bool _disposed;

        public void Dispose()
        {
            lock (_syncLock)
            {
                _disposed = true;

                const int noSamplesProcessedHResult = unchecked((int) 0xC00D4A44);

                try
                {
                    _writer.Finalize();
                }
                // This error happens if recording is stopped before any samples are written
                catch (SharpDXException e) when (e.HResult == noSamplesProcessedHResult) { }

                _writer.Dispose();

                // Dispose all buffers in the ring buffer
                for (int i = 0; i < BufferCount; i++)
                {
                    _copyTextures[i]?.Dispose();
                    _copyTextures[i] = null;

                    _samples[i]?.Dispose();
                    _samples[i] = null;

                    _mediaBuffers[i]?.Dispose();
                    _mediaBuffers[i] = null;
                }
            }
        }

        public void WriteFrame(IBitmapFrame Image)
        {
            ++_frameNumber;

            if (Image is RepeatFrame)
                return;

            IBitmapFrame InnerImage = Image.Unwrap();

            using (Image)
            {
                if (InnerImage is Texture2DFrame frame)
                {
                    Write(frame.Texture);
                }
                else
                {
                    using var buffer = MediaFactory.CreateMemoryBuffer(_bufferSize);
                    var data = buffer.Lock(out _, out _);

                    InnerImage.CopyTo(data);

                    buffer.CurrentLength = _bufferSize;

                    buffer.Unlock();

                    using var sample = MediaFactory.CreateVideoSampleFromSurface(null);
                    sample.AddBuffer(buffer);

                    Write(sample);
                }
            }
        }

        public bool SupportsAudio => true;

        long _audioWrittenBytes;

        public void WriteAudio(byte[] Buffer, int Offset, int Length)
        {
            using (var buffer = MediaFactory.CreateMemoryBuffer(Length))
            {
                var data = buffer.Lock(out _, out _);

                Marshal.Copy(Buffer, Offset, data, Length);

                buffer.CurrentLength = Length;

                buffer.Unlock();

                using var sample = MediaFactory.CreateVideoSampleFromSurface(null);
                sample.AddBuffer(buffer);

                // Calculate duration for this chunk based on its length
                var sampleDuration = Length * TenPower7 / _audioInBytesPerSecond;

                // Use expected timestamp based on total audio written
                // This provides smooth timestamps even if delivery is jittery
                sample.SampleTime = _audioWrittenBytes * TenPower7 / _audioInBytesPerSecond;
                sample.SampleDuration = sampleDuration;

                _writer.WriteSample(AudioStreamIndex, sample);

                _audioWrittenBytes += Length;
            }
        }
    }
}