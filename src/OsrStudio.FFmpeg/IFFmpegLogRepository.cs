using System.Collections.Generic;

namespace OsrStudio.FFmpeg
{
    public interface IFFmpegLogRepository : IEnumerable<IFFmpegLogEntry>
    {
        IFFmpegLogEntry CreateNew(string Name, string Args);

        void Remove(IFFmpegLogEntry Entry);
    }
}