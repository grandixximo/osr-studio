using System.IO;

namespace OsrStudio
{
    public interface IImagingSystem
    {
        IBitmapImage CreateBitmap(int Width, int Height);

        IBitmapImage LoadBitmap(string FileName);

        IBitmapImage LoadBitmap(Stream Stream);
    }
}