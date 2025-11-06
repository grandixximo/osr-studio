using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using OsrStudio.Audio;
using OsrStudio.Models;

// ReSharper disable MethodSupportsCancellation

namespace OsrStudio.Video
{
    /// <summary>
    /// Default implementation of <see cref="IRecorder"/> interface.
    /// Can output to <see cref="IVideoFileWriter"/> or <see cref="IAudioFileWriter"/>.
    /// </summary>
    public class Recorder : IRecorder
    {
        #region Fields
        IAudioProvider _audioProvider;
        IVideoFileWriter _videoWriter;
        IImageProvider _imageProvider;

        readonly int _frameRate;

        readonly Stopwatch _sw;

        readonly ManualResetEvent _continueCapturing;
        readonly CancellationTokenSource _cancellationTokenSource;
        readonly CancellationToken _cancellationToken;

        readonly Task _recordTask;

        readonly object _syncLock = new object();

        Task<bool> _frameWriteTask;
        Task _audioPumpTask;
        int _frameCount;
        long _audioBytesWritten;
        long _maxAudioBytes; // Maximum audio to write when stopping (prevents audio past last frame)
        readonly int _audioBytesPerFrame, _audioBytesPerSecond, _audioChunkBytes;
        // Audio chunk size: smaller chunks = smoother audio delivery to encoder
        // 20ms chunks provide better temporal consistency
        const int AudioChunkLengthMs = 20;
        byte[] _audioBuffer, _silenceBuffer;

        readonly IFpsManager _fpsManager;
        #endregion

        /// <summary>
        /// Creates a new instance of <see cref="IRecorder"/> writing to <see cref="IVideoFileWriter"/>.
        /// </summary>
        /// <param name="VideoWriter">The <see cref="IVideoFileWriter"/> to write to.</param>
        /// <param name="ImageProvider">The image source.</param>
        /// <param name="FrameRate">Video Frame Rate.</param>
        /// <param name="AudioProvider">The audio source. null = no audio.</param>
        public Recorder(IVideoFileWriter VideoWriter, IImageProvider ImageProvider, int FrameRate,
            IAudioProvider AudioProvider = null,
            IFpsManager FpsManager = null)
        {
            _videoWriter = VideoWriter ?? throw new ArgumentNullException(nameof(VideoWriter));
            _imageProvider = ImageProvider ?? throw new ArgumentNullException(nameof(ImageProvider));
            _audioProvider = AudioProvider;
            _fpsManager = FpsManager;

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            if (FrameRate <= 0)
                throw new ArgumentException("Frame Rate must be possitive", nameof(FrameRate));

            _frameRate = FrameRate;

            _continueCapturing = new ManualResetEvent(false);

            if (VideoWriter.SupportsAudio && AudioProvider != null)
            {
                var wf = AudioProvider.WaveFormat;

                _audioBytesPerFrame = (int) ((1.0 / FrameRate)
                                             * wf.SampleRate
                                             * wf.Channels
                                             * (wf.BitsPerSample / 8.0));

                _audioBytesPerSecond = _audioBytesPerFrame * FrameRate;
                _audioChunkBytes = (int) (_audioBytesPerSecond * (AudioChunkLengthMs / 1000.0));
            }
            else _audioProvider = null;

            _sw = new Stopwatch();

            _recordTask = Task.Factory.StartNew(async () => await DoRecord(), TaskCreationOptions.LongRunning);

            if (_audioProvider != null)
            {
                _audioPumpTask = Task.Factory.StartNew(AudioPumpLoop, _cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        async Task DoRecord()
        {
            try
            {
                var frameInterval = TimeSpan.FromSeconds(1.0 / _frameRate);
                _frameCount = 0;

                // Returns false when stopped

                while (_continueCapturing.WaitOne() && !_cancellationToken.IsCancellationRequested)
                {
                    var timestamp = _sw.Elapsed;

                    if (_frameWriteTask != null)
                    {
                        // Don't block waiting for encoder - check if it's done
                        if (_frameWriteTask.IsCompleted)
                        {
                            // If false, stop recording
                            if (!await _frameWriteTask)
                                return;

                            if (!WriteDuplicateFrame())
                                return;

                            _frameWriteTask = null;
                        }
                        else
                        {
                            // Encoder still busy - insert repeat frame to maintain timing
                            if (!AddFrame(RepeatFrame.Instance))
                                return;

                            ++_frameCount;
                            continue; // Skip starting new capture
                        }
                    }

                    _frameWriteTask = Task.Run(() => FrameWriter(timestamp));

                    var timeTillNextFrame = timestamp + frameInterval - _sw.Elapsed;

                    if (timeTillNextFrame > TimeSpan.Zero)
                        Thread.Sleep(timeTillNextFrame);
                }
            }
            catch (Exception e)
            {
                lock (_syncLock)
                {
                    if (!_disposed)
                    {
                        ErrorOccurred?.Invoke(e);

                        Dispose(false);
                    }
                }
            }
        }

        bool FrameWriter(TimeSpan Timestamp)
        {
            var editableFrame = _imageProvider.Capture();

            var frame = editableFrame.GenerateFrame(Timestamp);

            var success = AddFrame(frame);

            if (!success)
            {
                return false;
            }

            _fpsManager?.OnFrame();

            return true;
        }

        bool WriteDuplicateFrame()
        {
            var requiredFrames = _sw.Elapsed.TotalSeconds * _frameRate;
            var diff = requiredFrames - _frameCount;

            // Write atmost 1 duplicate frame
            if (diff >= 1)
            {
                if (!AddFrame(RepeatFrame.Instance))
                    return false;
            }

            return true;
        }

        bool AddFrame(IBitmapFrame Frame)
        {
            try
            {
                _videoWriter.WriteFrame(Frame);

                ++_frameCount;

                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        void WriteAudio()
        {
            if (_audioProvider == null)
            {
                return;
            }

            // Use elapsed time to determine audio budget to decouple from video frame pacing
            var shouldHaveWritten = (long)(_sw.Elapsed.TotalSeconds * _audioBytesPerSecond);

            // When stopping, don't write audio past the maximum (prevents audio extending past last frame)
            if (_maxAudioBytes > 0)
            {
                shouldHaveWritten = Math.Min(shouldHaveWritten, _maxAudioBytes);
            }

            // Already written more than enough, skip for now
            if (_audioBytesWritten >= shouldHaveWritten)
            {
                return;
            }

            var toWrite = (int)(shouldHaveWritten - _audioBytesWritten);

            // Write in consistent chunk sizes for smoother delivery
            // Round down to nearest chunk boundary
            toWrite = (toWrite / _audioChunkBytes) * _audioChunkBytes;

            // Must have at least one chunk to write
            if (toWrite < _audioChunkBytes)
            {
                return;
            }

            // Cap maximum write size to prevent large bursts
            var maxBurstBytes = _audioChunkBytes * 3;
            if (toWrite > maxBurstBytes)
            {
                toWrite = maxBurstBytes;
            }

            // Reallocate buffer as needed
            if (_audioBuffer == null || _audioBuffer.Length < toWrite)
            {
                _audioBuffer = new byte[toWrite];
            }

            var read = _audioProvider.Read(_audioBuffer, 0, toWrite);

            // Write whatever we got from the provider
            if (read > 0)
            {
                _videoWriter.WriteAudio(_audioBuffer, 0, read);
                _audioBytesWritten += read;
            }

            // Fill any remaining gap with silence to maintain synchronization
            // This ensures we stay on schedule even if audio provider is lagging
            var silenceToWrite = toWrite - read;

            if (silenceToWrite > 0)
            {
                // Reallocate silence buffer: An array of zeros.
                if (_silenceBuffer == null || _silenceBuffer.Length < silenceToWrite)
                {
                    _silenceBuffer = new byte[silenceToWrite];
                }

                _videoWriter.WriteAudio(_silenceBuffer, 0, silenceToWrite);
                _audioBytesWritten += silenceToWrite;
            }
        }

        void AudioPumpLoop()
        {
            // Set high thread priority for consistent audio delivery
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

            while (!_cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (_continueCapturing.WaitOne(0))
                    {
                        WriteAudio();
                    }
                }
                catch { }

                // Pump more frequently for smoother audio (every 10ms)
                Thread.Sleep(10);
            }
        }

        #region Dispose
        async void Dispose(bool TerminateRecord)
        {
            if (_disposed)
                return;

            _disposed = true;

            _cancellationTokenSource.Cancel();

            // Resume record loop if paused so it can exit
            _continueCapturing.Set();

            // Ensure all threads exit before disposing resources.
            if (TerminateRecord)
                _recordTask.Wait();

            try
            {
                if (_frameWriteTask != null)
                    await _frameWriteTask.ConfigureAwait(false);
            }
            catch { }

            // Now that all video frames are written, refine the maximum audio bytes
            // based on actual frame count to ensure perfect audio/video sync at the end
            if (_audioProvider != null && _frameCount > 0)
            {
                var requiredAudioBytes = (long)_frameCount * _audioBytesPerFrame;

                // Use the actual frame count to set precise audio limit
                // This ensures audio doesn't extend past the last video frame
                if (_maxAudioBytes == 0 || requiredAudioBytes < _maxAudioBytes)
                {
                    _maxAudioBytes = requiredAudioBytes;
                }
            }

            try { _audioPumpTask?.Wait(2000); } catch { }

            if (_audioProvider != null)
            {
                _audioProvider.Stop();

                // Pad trailing audio with silence to match last video frame boundary
                try
                {
                    if (_videoWriter != null)
                    {
                        var requiredAudioBytes = (long)_frameCount * _audioBytesPerFrame;
                        var missingBytes = requiredAudioBytes - _audioBytesWritten;

                        if (missingBytes > 0)
                        {
                            var padBuffer = _silenceBuffer;
                            if (padBuffer == null || padBuffer.Length < _audioChunkBytes)
                            {
                                padBuffer = new byte[_audioChunkBytes];
                            }

                            while (missingBytes > 0)
                            {
                                var toWrite = (int)Math.Min(missingBytes, (long)_audioChunkBytes);
                                _videoWriter.WriteAudio(padBuffer, 0, toWrite);
                                _audioBytesWritten += toWrite;
                                missingBytes -= toWrite;
                            }
                        }
                    }
                }
                catch { }

                _audioProvider.Dispose();
                _audioProvider = null;
            }

            _imageProvider?.Dispose();
            _imageProvider = null;

            _videoWriter.Dispose();
            _videoWriter = null;

            _audioBuffer = _silenceBuffer = null;

            _continueCapturing.Dispose();
        }

        /// <summary>
        /// Frees all resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            lock (_syncLock)
            {
                Dispose(true);
            }
        }

        bool _disposed;

        /// <summary>
        /// Fired when an error occurs
        /// </summary>
        public event Action<Exception> ErrorOccurred;

        void ThrowIfDisposed()
        {
            lock (_syncLock)
            {
                if (_disposed)
                    throw new ObjectDisposedException("this");
            }
        }
        #endregion

        /// <summary>
        /// Start Recording.
        /// </summary>
        public void Start()
        {
            ThrowIfDisposed();

            _sw?.Start();

            _audioProvider?.Start();
            
            _continueCapturing?.Set();
        }

        /// <summary>
        /// Stop Recording.
        /// </summary>
        public void Stop()
        {
            ThrowIfDisposed();

            _continueCapturing?.Reset();
            _audioProvider?.Stop();

            _sw?.Stop();

            // Set preliminary maximum audio based on stopwatch time and frame rate
            // This prevents audio pump from writing past video duration
            // Will be refined in Dispose() based on actual final frame count
            if (_audioProvider != null && _sw != null)
            {
                var estimatedFrames = (long)(_sw.Elapsed.TotalSeconds * _frameRate);
                _maxAudioBytes = estimatedFrames * _audioBytesPerFrame;
            }
        }
    }
}
