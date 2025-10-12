namespace OsrStudio.Models
{
    public interface IFpsManager
    {
        void OnFrame();

        int Fps { get; }
    }
}