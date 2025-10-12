using System.Windows;
using OsrStudio.ViewModels;

namespace OsrStudio
{
    public partial class YouTubeUploaderWindow
    {
        public YouTubeUploaderWindow()
        {
            InitializeComponent();

            Closing += async (S, E) =>
            {
                if (DataContext is YouTubeUploaderViewModel vm)
                {
                    if (!await vm.Cancel())
                    {
                        E.Cancel = true;
                    }
                }
            };
        }

        public async void Open(string FileName)
        {
            if (DataContext is YouTubeUploaderViewModel vm)
            {
                await vm.Init(FileName);
            }
        }

        void Cancel_Click(object Sender, RoutedEventArgs E)
        {
            Close();
        }
    }
}
