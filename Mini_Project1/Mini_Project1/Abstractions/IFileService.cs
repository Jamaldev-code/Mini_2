namespace Mini_Project1.Abstractions
{
    internal interface IFileService
    {
        List<T> Read<T>(string filePath);
        void Write<T>(string filePath, List<T> data);
    }
}
