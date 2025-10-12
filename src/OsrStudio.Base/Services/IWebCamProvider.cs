using System.Collections.Generic;

namespace OsrStudio.Webcam
{
    public interface IWebCamProvider
    {
        IEnumerable<IWebcamItem> GetSources();
    }
}
