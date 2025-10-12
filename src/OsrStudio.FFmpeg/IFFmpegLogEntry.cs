using System;

namespace OsrStudio.FFmpeg
{
    public interface IFFmpegLogEntry
    {
        void Write(string Line);

        string GetCompleteLog();

        event Action<int> ProgressChanged;
    }
}