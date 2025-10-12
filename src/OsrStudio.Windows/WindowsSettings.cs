using OsrStudio.Windows.MediaFoundation;

namespace OsrStudio.Windows
{
    public class WindowsSettings : PropertyStore
    {
        public MfSettings MediaFoundation { get; } = new MfSettings();
    }
}