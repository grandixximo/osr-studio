using SharpDX.MediaFoundation;

namespace OsrStudio.Windows.MediaFoundation
{
    public static class MfManager
    {
        public static void Startup()
        {
            MediaManager.Startup();
        }
        public static void Shutdown()
        {
            MediaFactory.Shutdown();
        }
    }
}