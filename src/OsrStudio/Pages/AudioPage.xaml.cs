using OsrStudio.Models;
using OsrStudio.ViewModels;

namespace OsrStudio
{
    public partial class AudioPage
    {
        public AudioPage()
        {
            IsVisibleChanged += (S, E) =>
            {
                var audioSourceVm = ServiceProvider.Get<AudioSourceViewModel>();

                audioSourceVm.ListeningPeakLevel = IsVisible;
            };

            InitializeComponent();
            
            ServiceProvider.Get<MainViewModel>().Refreshed += () =>
            {
                AudioSourcesPanel?.Shake();
            };
        }
    }
}
