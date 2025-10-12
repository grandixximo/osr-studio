using System.Windows;

namespace OsrStudio
{
    public partial class CollapsedBar
    {
        public CollapsedBar()
        {
            InitializeComponent();
        }

        void OpenSettings(object Sender, RoutedEventArgs E)
        {
            SettingsWindow.ShowInstance();
        }
    }
}
