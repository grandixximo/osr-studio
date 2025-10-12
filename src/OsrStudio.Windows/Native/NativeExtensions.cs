using System.Drawing;

namespace OsrStudio.Native
{
    static class NativeExtensions
    {
        public static Rectangle ToRectangle(this RECT R) => Rectangle.FromLTRB(R.Left, R.Top, R.Right, R.Bottom);
    }
}