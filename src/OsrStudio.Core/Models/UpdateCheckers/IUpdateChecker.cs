using System;
using System.Threading.Tasks;

namespace OsrStudio.Models
{
    public interface IUpdateChecker
    {
        void GoToDownloadsPage();

        Task<Version> Check();

        string BuildName { get; }
    }
}