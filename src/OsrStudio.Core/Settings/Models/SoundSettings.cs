using System.Collections.Generic;

namespace OsrStudio.Audio
{
    public class SoundSettings : PropertyStore
    {
        public Dictionary<SoundKind, string> Items { get; } = new Dictionary<SoundKind, string>();
    }
}