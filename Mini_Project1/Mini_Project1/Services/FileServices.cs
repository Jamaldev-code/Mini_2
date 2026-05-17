using Mini_Project1.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace Mini_Project1.Services
{
    internal class FileServices:IFileService
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public List<T> Read<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<T>();

            var json = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(json))
                return new List<T>();

            return JsonSerializer.Deserialize<List<T>>(json, _options)
                ?? new List<T>();
        }

        public void Write<T>(string filePath, List<T> data)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            var json = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(filePath, json);
        }
     
    }
}
