using System;
using System.Windows;

namespace OsrStudio
{
    public partial class LayerFrame
    {
        public LayerFrame()
        {
            InitializeComponent();
        }

        public event Action<Rect> PositionUpdated;

        public void RaisePositionChanged(Rect Rect)
        {
            PositionUpdated?.Invoke(Rect);
        }
    }
}