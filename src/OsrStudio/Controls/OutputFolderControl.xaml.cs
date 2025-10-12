using System.Windows.Input;
using OsrStudio.ViewModels;

namespace OsrStudio
{
    public partial class OutputFolderControl
    {
        public OutputFolderControl()
        {
            InitializeComponent();
        }

        void SelectTargetFolder(object Sender, MouseButtonEventArgs E)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.SelectOutputFolderCommand.ExecuteIfCan();
            }
        }
    }
}
