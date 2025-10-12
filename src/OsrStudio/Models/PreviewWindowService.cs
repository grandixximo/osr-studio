using System;
using System.Windows;
using System.Windows.Interop;
using OsrStudio.Windows.DirectX;
using OsrStudio.Windows.Gdi;
using Reactive.Bindings.Extensions;
using SharpDX.Direct3D9;

namespace OsrStudio.Video
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PreviewWindowService : IPreviewWindow
    {
        D3D9PreviewAssister _d3D9PreviewAssister;
        IntPtr _backBufferPtr;
        Texture _texture;
        readonly VisualSettings _visualSettings;

        public void Show()
        {
            _visualSettings.Expanded = true;
        }

        public bool IsVisible { get; private set; }

        public PreviewWindowService(VisualSettings VisualSettings)
        {
            _visualSettings = VisualSettings;

            VisualSettings.ObserveProperty(M => M.Expanded)
                .Subscribe(M => IsVisible = M);
        }

        IBitmapFrame _lastFrame;

        public void Display(IBitmapFrame Frame)
        {
            if (Frame is RepeatFrame)
                return;

            if (!IsVisible)
            {
                Frame.Dispose();
                return;
            }

            var win = PreviewWindow.Instance;
            
            if (win == null)
            {
                Frame.Dispose();
                return;
            }

            win.Dispatcher.Invoke(() =>
            {
                _lastFrame?.Dispose();
                _lastFrame = Frame;

                Frame = Frame.Unwrap();

                switch (Frame)
                {
                    case DrawingFrame drawingFrame:
                        try
                        {
                            // TODO: Preview is not shown during Webcam only recordings
                            // This check swallows errors
                            var h = drawingFrame.Bitmap.Height;

                            if (h == 0)
                                return;
                        }
                        catch { return; }

                        // Convert System.Drawing.Bitmap to WPF BitmapSource
                        using (var memory = new System.IO.MemoryStream())
                        {
                            drawingFrame.Bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                            memory.Position = 0;
                            var bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = memory;
                            bitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                            bitmapImage.EndInit();
                            bitmapImage.Freeze();
                            win.DisplayImage.Source = bitmapImage;
                        }
                        break;

                    case Texture2DFrame texture2DFrame:
                        // D3D preview not supported in classic UI separate window mode
                        break;
                }
            });
        }

        void Invalidate(IntPtr BackBufferPtr, int Width, int Height)
        {
            // D3D preview not supported in classic UI separate window mode
        }

        public void Dispose()
        {
            var win = PreviewWindow.Instance;

            if (win == null)
                return;

            win.Dispatcher.Invoke(() =>
            {
                win.DisplayImage.Source = null;

                _lastFrame?.Dispose();
                _lastFrame = null;

                if (_d3D9PreviewAssister != null)
                {
                    _d3D9PreviewAssister.Dispose();
                    _d3D9PreviewAssister = null;
                }
            });
        }
    }
}
