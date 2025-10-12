using System;
using OsrStudio.FFmpeg;

namespace OsrStudio.Fakes
{
    // ReSharper disable once ClassNeverInstantiated.Global
    class FakeFFmpegViewsProvider : IFFmpegViewsProvider
    {
        public void ShowLogs()
        {
        }

        public void ShowUnavailable()
        {
            Console.Error.WriteLine("FFmpeg is not available.\nYou can install ffmpeg by calling: osr-cli ffmpeg --install [path]");
        }

        public void ShowDownloader()
        {
        }

        public void PickFolder()
        {
        }
    }
}