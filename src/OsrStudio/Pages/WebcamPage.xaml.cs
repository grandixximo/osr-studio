using System;
using System.ComponentModel;
using System.Drawing;
using System.Reactive.Linq;
using WSize = System.Windows.Size;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using OsrStudio.ViewModels;
using OsrStudio.Webcam;
using OsrStudio.Windows.Gdi;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace OsrStudio
{
    public partial class WebcamPage
    {
        public WebcamPage()
        {
            InitializeComponent();
            
            ServiceProvider.Get<MainViewModel>().Refreshed += () =>
            {
                WebcamComboBox?.Shake();
            };
        }
    }
}
