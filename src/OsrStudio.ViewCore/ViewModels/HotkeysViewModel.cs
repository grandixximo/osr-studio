using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using OsrStudio.Hotkeys;
using Reactive.Bindings;

namespace OsrStudio.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HotkeysViewModel
    {
        public ReadOnlyObservableCollection<Hotkey> Hotkeys { get; }

        public IEnumerable<Service> AllServices => HotKeyManager.AllServices;

        public HotkeysViewModel(HotKeyManager HotKeyManager)
        {
            Hotkeys = HotKeyManager.Hotkeys;

            ResetCommand = new ReactiveCommand()
                .WithSubscribe(HotKeyManager.Reset);

            AddCommand = new ReactiveCommand()
                .WithSubscribe(HotKeyManager.Add);

            RemoveCommand = new ReactiveCommand()
                .WithSubscribe(M =>
                {
                    if (M is Hotkey hotkey)
                    {
                        HotKeyManager.Remove(hotkey);
                    }
                });
        }

        public ICommand ResetCommand { get; }

        public ICommand AddCommand { get; }

        public ICommand RemoveCommand { get; }
    }
}