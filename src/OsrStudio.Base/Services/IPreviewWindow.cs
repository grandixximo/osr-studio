using System;

namespace OsrStudio.Video
{
    public interface IPreviewWindow : IDisposable
    {
        void Display(IBitmapFrame Frame);

        void Show();

        bool IsVisible { get; }
    }
}