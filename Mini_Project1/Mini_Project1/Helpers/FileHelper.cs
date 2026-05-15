using System.Text.Json;


namespace Mini_Project1.Helpers
{
    internal class FileHelper
    {
        public static void WriteToFile<T>(string path, T data)
        {
            string result = JsonSerializer.Serialize(data);

            File.WriteAllText(path, result);
        }

        public static T ReadFromFile<T>(string path)
        {
            if (!File.Exists(path))
            {
                return default;
            }

            string data = File.ReadAllText(path);

            return JsonSerializer.Deserialize<T>(data);
        }
    }
}
