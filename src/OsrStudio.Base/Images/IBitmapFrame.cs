using System;

namespace OsrStudio
{
    public interface IBitmapFrame : IDisposable
    {
        int Width { get; }

        int Height { get; }

        void CopyTo(byte[] Buffer);

        void CopyTo(IntPtr Buffer);

        TimeSpan Timestamp { get; }
    }
}