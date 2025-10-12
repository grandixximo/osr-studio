using System.Windows;
using OsrStudio.Models;
using OsrStudio.Video;
using OsrStudio.ViewModels;
using Settings = OsrStudio.Settings;

namespace OsrStudio
{
    public partial class WebCamWindow
    {
        WebCamWindow()
        {
            InitializeComponent();
            
            Closing += (S, E) =>
            {
                Hide();

                E.Cancel = true;
            };
        }

        static WebCamWindow _instance;
        static readonly object _lockObject = new object();

        public static WebCamWindow Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            // Ensure creation on UI thread
                            if (Application.Current?.Dispatcher.CheckAccess() == true)
                            {
                                _instance = new WebCamWindow();
                            }
                            else if (Application.Current != null)
                            {
                                Application.Current.Dispatcher.Invoke(() => _instance = new WebCamWindow());
                            }
                        }
                    }
                }
                return _instance;
            }
        }

        public WebcamControl GetWebCamControl() => WebCameraControl;

        void CloseButton_Click(object Sender, RoutedEventArgs E) => Close();
        
        async void CaptureImage_OnClick(object Sender, RoutedEventArgs E)
        {
            try
            {
                // Get image from webcam control
                var bitmapLoader = ServiceProvider.Get<IBitmapLoader>();
                var img = WebCameraControl.Capture?.GetFrame(bitmapLoader);
                
                if (img != null)
                {
                    var screenShotViewModel = ServiceProvider.Get<ScreenShotViewModel>();
                    var settings = ServiceProvider.Get<Settings>();
                    // Use DiskWriter to save the screenshot
                    await screenShotViewModel.DiskWriter.Save(img, settings.ScreenShots.ImageFormat, null);
                }
            }
            catch { }
        }
    }
}