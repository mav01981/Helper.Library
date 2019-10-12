namespace Helper.FileConvertor.Interfaces
{
    public interface IByteConvertor
    {
        byte[] ConvertToBytes(string path);
        void ConvertBytesToFile(string filePath, FileExtensionsEnums fileEnum, byte[] bytes);
    }
}
