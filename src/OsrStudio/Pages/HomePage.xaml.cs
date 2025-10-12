using OsrStudio.ViewModels;

namespace OsrStudio
{
    public partial class HomePage
    {
        public HomePage()
        {
            InitializeComponent();
            
            ServiceProvider.Get<MainViewModel>().Refreshed += () =>
            {
                AudioDropdown?.Shake();
                VideoWriterComboBox?.Shake();
                VideoSourcesList?.Shake();
            };
        }
    }
}
