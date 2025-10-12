using NAudio.CoreAudioApi;

namespace OsrStudio.Audio
{
    class WasapiCaptureProvider : NAudioProvider
    {
        public WasapiCaptureProvider(MMDevice Device)
            : base(new WasapiCapture(Device)) { }
    }
}