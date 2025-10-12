using System.Windows;
using OsrStudio.Views;

namespace OsrStudio
{
    public partial class AboutPage
    {
        void ViewLicenses(object Sender, RoutedEventArgs Args)
        {
            LicensesWindow.ShowInstance();
        }

        void ViewCrashLogs(object Sender, RoutedEventArgs Args)
        {
            CrashLogsWindow.ShowInstance();
        }
    }
}
