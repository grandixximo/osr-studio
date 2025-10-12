using OsrStudio.Models;
using OsrStudio.Audio;
using OsrStudio.FFmpeg;
using OsrStudio.Hotkeys;
using OsrStudio.Loc;
using OsrStudio.MouseKeyHook;
using OsrStudio.SharpAvi;
using OsrStudio.Video;
using OsrStudio.ViewModels;
using OsrStudio.Webcam;
using OsrStudio.Windows;
using OsrStudio.YouTube;

namespace OsrStudio
{
    public class CoreModule : IModule
    {
        public void OnLoad(IBinder Binder)
        {
            Binder.Bind<IAudioWriterItem, WaveItem>();

            WindowsModule.Load(Binder);

            FFmpegModule.Load(Binder);

            BindViewModels(Binder);
            BindSettings(Binder);
            BindImageWriters(Binder);
            BindVideoWriterProviders(Binder);
            BindVideoSourceProviders(Binder);
            BindAudioSource(Binder);
            BindUpdateChecker(Binder);

            // Recent
            Binder.Bind<IRecentList, RecentListRepository>();
            Binder.Bind<IRecentItemSerializer, FileRecentSerializer>();

            Binder.Bind<IIconSet, MaterialDesignIcons>();
            Binder.Bind<IYouTubeApiKeys, ApiKeys>();

            Binder.BindSingleton<HotKeyManager>();

            Binder.Bind<ILocalizationProvider>(() => LanguageManager.Instance);

            Binder.Bind<IFpsManager, FpsManager>();
        }

        public void Dispose()
        {
            WindowsModule.Unload();
        }

        static void BindImageWriters(IBinder Binder)
        {
            Binder.BindAsInterfaceAndClass<IImageWriterItem, DiskWriter>();
            Binder.BindAsInterfaceAndClass<IImageWriterItem, ClipboardWriter>();
            Binder.BindAsInterfaceAndClass<IImageWriterItem, EditorWriter>();
        }

        static void BindViewModels(IBinder Binder)
        {
            Binder.BindSingleton<TimerModel>();
            Binder.BindSingleton<ScreenShotModel>();
            Binder.BindSingleton<RecordingModel>();
            Binder.BindSingleton<WebcamModel>();
            Binder.BindSingleton<KeymapViewModel>();
        }

        static void BindUpdateChecker(IBinder Binder)
        {
            var version = ServiceProvider.AppVersion;

            if (version?.Major == 0)
            {
                Binder.Bind<IUpdateChecker, DevUpdateChecker>();
            }
            else Binder.Bind<IUpdateChecker, UpdateChecker>();
        }

        static void BindAudioSource(IBinder Binder)
        {
            Binder.Bind<IAudioSource, NAudioSource>();
        }

        static void BindVideoSourceProviders(IBinder Binder)
        {
            Binder.BindAsInterfaceAndClass<IVideoSourceProvider, NoVideoSourceProvider>();
            Binder.BindAsInterfaceAndClass<IVideoSourceProvider, AroundMouseSourceProvider>();
            Binder.BindAsInterfaceAndClass<IVideoSourceProvider, WebcamSourceProvider>();
            Binder.BindAsInterfaceAndClass<IVideoSourceProvider, FullScreenSourceProvider>();
            Binder.BindAsInterfaceAndClass<IVideoSourceProvider, ScreenSourceProvider>();
            Binder.BindAsInterfaceAndClass<IVideoSourceProvider, WindowSourceProvider>();
            Binder.BindAsInterfaceAndClass<IVideoSourceProvider, RegionSourceProvider>();
        }

        static void BindVideoWriterProviders(IBinder Binder)
        {
            Binder.BindAsInterfaceAndClass<IVideoWriterProvider, SharpAviWriterProvider>();
            Binder.BindAsInterfaceAndClass<IVideoWriterProvider, DiscardWriterProvider>();
        }

        static void BindSettings(IBinder Binder)
        {
            Binder.BindSingleton<Settings>();
            Binder.Bind(() => Binder.Get<Settings>().Audio);
            Binder.Bind(() => Binder.Get<Settings>().Proxy);
            Binder.Bind(() => Binder.Get<Settings>().Sounds);
            Binder.Bind(() => Binder.Get<Settings>().Steps);
            Binder.Bind(() => Binder.Get<Settings>().Video);
            Binder.Bind(() => Binder.Get<Settings>().UI);
            Binder.Bind(() => Binder.Get<Settings>().WebcamOverlay);
        }
    }
}