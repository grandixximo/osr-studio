using System.Collections.Generic;

namespace OsrStudio.Video
{
    public interface IVideoWriterProvider : IEnumerable<IVideoWriterItem>
    {
        string Name { get; }

        string Description { get; }

        IVideoWriterItem ParseCli(string Cli);
    }
}