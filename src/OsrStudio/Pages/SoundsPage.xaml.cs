using System.Windows;
using System.Windows.Input;
using OsrStudio.ViewModels;

namespace OsrStudio
{
    public partial class SoundsPage
    {
        public SoundsPage()
        {
            InitializeComponent();
        }

        void SetFile(object Sender, MouseButtonEventArgs E)
        {
            if (Sender is FrameworkElement element && element.DataContext is SoundsViewModelItem vm)
            {
                vm.SetCommand.ExecuteIfCan();
            }
        }
    }
}
