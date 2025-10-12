using System;
using OsrStudio.Loc;
using OsrStudio.Models;

namespace OsrStudio.Fakes
{
    // ReSharper disable once ClassNeverInstantiated.Global
    class FakeSystemTray : ISystemTray
    {
        readonly ILocalizationProvider _loc;

        public FakeSystemTray(ILocalizationProvider Loc)
        {
            _loc = Loc;
        }

        public void HideNotification() { }

        public void ShowScreenShotNotification(string FilePath)
        {
            // ReSharper disable once LocalizableElement
            Console.WriteLine($"{_loc.ScreenShotSaved}: {FilePath}");
        }

        public void ShowNotification(INotification Notification) { }
    }
}
