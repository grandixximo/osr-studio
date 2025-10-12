using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Captura.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DevUpdateChecker : IUpdateChecker
    {
        readonly ProxySettings _proxySettings;
        readonly Version _currentVersion;

        public DevUpdateChecker(ProxySettings ProxySettings)
        {
            _proxySettings = ProxySettings;

            _currentVersion = ServiceProvider.AppVersion;
        }

        public void GoToDownloadsPage()
        {
            Process.Start(DownloadsUrl);
        }

        const string DownloadsUrl = "https://github.com/grandixximo/osr-studio/releases";
        const string LatestReleaseUrl = "https://api.github.com/repos/grandixximo/osr-studio/releases/latest";

        public async Task<Version> Check()
        {
            using (var w = new WebClient { Proxy = _proxySettings.GetWebProxy() })
            {
                // User Agent header required by GitHub API
                w.Headers.Add("user-agent", "OSR-Studio");

                var result = await w.DownloadStringTaskAsync(LatestReleaseUrl);

                var jObj = JObject.Parse(result);

                // tag_name format: v10.0.0 or v10.0.0-beta1
                var tagName = jObj["tag_name"].ToString();
                var versionString = tagName.TrimStart('v');
                
                // Parse version, handling pre-release tags
                var version = Version.Parse(versionString.Split('-')[0]);

                if (version > _currentVersion)
                {
                    return version;
                }
            }

            return null;
        }

        public string BuildName => _currentVersion.Build == 0 ? "DEV" : "CI";
    }
}