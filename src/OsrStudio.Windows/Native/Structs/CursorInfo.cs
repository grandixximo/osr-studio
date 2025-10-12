using System;
using System.Drawing;
using System.Runtime.InteropServices;
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace OsrStudio.Native
{
    [StructLayout(LayoutKind.Sequential)]
    struct CursorInfo
    {
        public int cbSize;
        public int flags;
        public IntPtr hCursor;
        public Point ptScreenPos;
    }
}