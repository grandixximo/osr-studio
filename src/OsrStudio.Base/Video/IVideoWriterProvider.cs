using System.Collections.Generic;
using Captura.Video;

namespace OsrStudio.Video
{
    public interface IVideoWriterProvider : IEnumerable<IVideoWriterItem>
    {
        string Name { get; }

        string Description { get; }

        IVideoWriterItem ParseCli(string Cli);
    }
}