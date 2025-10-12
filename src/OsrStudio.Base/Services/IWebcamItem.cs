using System;

namespace OsrStudio.Webcam
{
    public interface IWebcamItem
    {
        string Name { get; }

        IWebcamCapture BeginCapture(Action OnClick);
    }
}