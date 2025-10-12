using System;

namespace OsrStudio
{
    public interface IRemoveRequester
    {
        event Action RemoveRequested;
    }
}