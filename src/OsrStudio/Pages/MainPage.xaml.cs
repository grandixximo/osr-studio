using System.Windows;

namespace OsrStudio
{
    public partial class MainPage
    {
        void OpenSettings(object Sender, RoutedEventArgs E)
        {
            SettingsWindow.ShowInstance();
        }
    }
}