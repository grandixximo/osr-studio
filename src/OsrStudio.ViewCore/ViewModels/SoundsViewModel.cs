using System.Collections.Generic;
using System.Linq;
using OsrStudio.Audio;
using OsrStudio.Models;

namespace OsrStudio.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SoundsViewModel : NotifyPropertyChanged
    {
        public IReadOnlyCollection<SoundsViewModelItem> Items { get; }

        public SoundsViewModel(IDialogService DialogService, SoundSettings Settings)
        {
            Items = new[]
            {
                SoundKind.Start,
                SoundKind.Stop,
                SoundKind.Pause,
                SoundKind.Shot,
                SoundKind.Error,
                SoundKind.Notification
            }.Select(Kind => new SoundsViewModelItem(Kind, DialogService, Settings)).ToList();
        }
    }
}