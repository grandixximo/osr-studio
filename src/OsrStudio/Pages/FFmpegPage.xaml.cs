using System.Windows;
using System.Windows.Input;
using OsrStudio.ViewModels;
using OsrStudio.Views;

namespace OsrStudio
{
    public partial class FFmpegPage
    {
        void FFmpegDownload(object Sender, RoutedEventArgs E)
        {
            FFmpegDownloaderWindow.ShowInstance();
        }

        void SelectFFmpegFolder(object Sender, MouseButtonEventArgs E)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.SelectFFmpegFolderCommand.ExecuteIfCan();
            }
        }
    }
}
