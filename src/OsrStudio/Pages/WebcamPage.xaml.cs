using System;
using System.Windows;
using OsrStudio.ViewModels;
using OsrStudio.Webcam;
using OsrStudio.Windows.Gdi;

namespace OsrStudio
{
    public partial class WebcamPage
    {
        public WebcamPage()
        {
            InitializeComponent();
        }

        async void CaptureImage_OnClick(object Sender, RoutedEventArgs E)
        {
            try
            {
                var screenShotModel = ServiceProvider.Get<ScreenShotModel>();
                var webcamModel = ServiceProvider.Get<WebcamModel>();
                
                // Get the current webcam capture
                var webcamCapture = webcamModel.InitCapture();
                
                if (webcamCapture?.Value != null)
                {
                    var img = webcamCapture.Value.Capture(GraphicsBitmapLoader.Instance);

                    if (img != null)
                    {
                        await screenShotModel.SaveScreenShot(img);
                    }
                    
                    // Release the capture reference we acquired
                    webcamModel.ReleaseCapture();
                }
                else
                {
                    ServiceProvider.MessageProvider?.ShowError("No webcam is currently active.\n\nPlease select a webcam first.", "No Webcam");
                }
            }
            catch (Exception ex)
            {
                ServiceProvider.MessageProvider?.ShowError($"Failed to capture image:\n\n{ex.Message}", "Capture Error");
            }
        }
    }
}
