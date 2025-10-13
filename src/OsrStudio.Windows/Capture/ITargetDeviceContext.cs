using System;

namespace OsrStudio.Video
{
    interface ITargetDeviceContext : IDisposable
    {
        IntPtr GetDC();

        IBitmapFrame DummyFrame { get; }

        IEditableFrame GetEditableFrame();
    }
}
