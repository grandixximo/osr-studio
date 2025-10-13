using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using OsrStudio.Models;
using OsrStudio.ViewModels;

namespace OsrStudio.Views
{
    public partial class FFmpegDownloaderWindow
    {
        public FFmpegDownloaderWindow()
        {
            InitializeComponent();

            if (DataContext is FFmpegDownloadViewModel vm)
            {
                Closing += async (S, E) =>
                {
                    if (!await vm.Cancel())
                    {
                        E.Cancel = true;
                    }
                };

                vm.ProgressChanged += P =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
                        TaskbarItemInfo.ProgressValue = P / 100.0;
                    });
                };

                vm.AfterDownload += Success =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        TaskbarItemInfo.ProgressState = Success ? TaskbarItemProgressState.None : TaskbarItemProgressState.Error;
                        TaskbarItemInfo.ProgressValue = 1;
                    });
                };
            }
        }

        void CloseButton_Click(object Sender, RoutedEventArgs E) => Close();

        public static void ShowInstance()
        {
            new FFmpegDownloaderWindow().ShowAndFocus();
        }

        void SelectTargetFolder(object Sender, MouseButtonEventArgs E)
        {
            if (DataContext is FFmpegDownloadViewModel vm)
            {
                vm.SelectFolderCommand.ExecuteIfCan();
            }
        }
    }
}
