using System.Drawing;

namespace OsrStudio.Video
{
    public interface IScreen
    {
        Rectangle Rectangle { get; }

        string DeviceName { get; }
    }
}