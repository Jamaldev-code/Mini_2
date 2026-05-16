using System.Text.Json;
using System.Text.Json.Serialization;


namespace Mini_Project1.Services
{
    internal class FileServices
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            // Enum dəyərlərini rəqəm əvəzinə ad kimi saxlayır (Pending, Confirmed...)
            Converters = { new JsonStringEnumConverter() }
        };

        // ──────────────────────────────────────────────
        //  Fayl yolları (sabit string-lər)
        // ──────────────────────────────────────────────

        public const string ProductsFile = "Repository\\products.json";
        public const string OrdersFile = "Repository\\orders.json";

        // ──────────────────────────────────────────────
        //  Oxuma metodu
        // ──────────────────────────────────────────────

        /// <summary>
        /// Göstərilən faylı oxuyub T tipli siyahı qaytarır.
        /// Fayl mövcud deyilsə və ya boşdursa boş siyahı qaytarılır.
        /// </summary>
        /// <typeparam name="T">Siyahı elementi tipi (məs. Product, Order)</typeparam>
        /// <param name="filePath">Faylın adı / yolu</param>
        public static List<T> Read<T>(string Path)
        {
            // Fayl yoxdursa boş siyahı qaytar
            if (!File.Exists(Path))
                return new List<T>();

            var json = File.ReadAllText(Path);

            // Fayl boş stringdirsə boş siyahı qaytar
            if (string.IsNullOrWhiteSpace(json))
                return new List<T>();

            // JSON-u deserializasiya et; null gəlsə boş siyahı istifadə edilir
            return JsonSerializer.Deserialize<List<T>>(json, _options) ?? new List<T>();
        }

        // ──────────────────────────────────────────────
        //  Yazma metodu
        // ──────────────────────────────────────────────

        /// <summary>
        /// T tipli siyahını JSON formatında göstərilən fayla yazır.
        /// Fayl mövcuddursa üzərinə yazılır (overwrite).
        /// </summary>
        public static void Write<T>(string filePath, List<T> data)
        {
            Directory.CreateDirectory("Repository");
            var json = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(filePath, json);
        }
    }
}
