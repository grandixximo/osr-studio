using System;

namespace OsrStudio
{
    public interface IModule : IDisposable
    {
        void OnLoad(IBinder Binder);
    }
}