namespace OsrStudio
{
    public interface INV12Frame : IBitmapFrame
    {
        void CopyNV12To(byte[] Buffer);
    }
}