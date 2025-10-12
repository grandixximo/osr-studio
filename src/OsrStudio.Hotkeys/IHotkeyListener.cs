using System;

namespace OsrStudio.Hotkeys
{
    public interface IHotkeyListener
    {
        event Action<int> HotkeyReceived;
    }
}