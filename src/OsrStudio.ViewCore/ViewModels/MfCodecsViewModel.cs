using System.Collections.Generic;
using System.Linq;
using OsrStudio.Video;
using OsrStudio.Windows;
using OsrStudio.Windows.MediaFoundation;

namespace OsrStudio.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MfCodecsViewModel : NotifyPropertyChanged
    {
        public MfSettings Settings { get; }

        readonly MfWriterProvider _mfWriterProvider;

        public MfCodecsViewModel(WindowsSettings WindowsSettings, IEnumerable<IVideoWriterProvider> WriterProviders)
        {
            this.Settings = WindowsSettings.MediaFoundation;
            _mfWriterProvider = WriterProviders.OfType<MfWriterProvider>().FirstOrDefault();
        }

        public IEnumerable<string> AvailableEncoders
        {
            get
            {
                if (_mfWriterProvider == null)
                    return Enumerable.Empty<string>();

                return _mfWriterProvider
                    .Select(item => item.ToString())
                    .Distinct();
            }
        }
    }
}