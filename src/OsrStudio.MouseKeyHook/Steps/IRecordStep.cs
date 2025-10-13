using System;
using System.Drawing;

namespace OsrStudio.MouseKeyHook.Steps
{
    interface IRecordStep
    {
        void Draw(IEditableFrame Editor, Func<Point, Point> PointTransform);

        bool Merge(IRecordStep NextStep);
    }
}
