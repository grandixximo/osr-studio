using System;
using System.Collections.ObjectModel;

namespace OsrStudio.Models
{
    public interface IRecentList : IDisposable
    {
        void Add(IRecentItem RecentItem);

        ReadOnlyObservableCollection<IRecentItem> Items { get; }

        void Clear();
    }
}