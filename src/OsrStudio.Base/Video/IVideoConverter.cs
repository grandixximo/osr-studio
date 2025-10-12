using System;
using System.Threading.Tasks;
using Captura.Video;

namespace OsrStudio.Video
{
    public interface IVideoConverter
    {
        string Name { get; }

        string Extension { get; }

        Task StartAsync(VideoConverterArgs Args, IProgress<int> Progress);
    }
}
