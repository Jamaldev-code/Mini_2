using Mini_Project1.Abstractions;
using Mini_Project1.Constants;

namespace Mini_Project1.Services
{
    /// <summary>
    /// Kateqoriyaları idarə edir: seçim, əlavə, silmə.
    /// </summary>
    internal class CategoryService
    {
        private readonly IFileService _fileService;
        private List<string> _categories;

        public CategoryService(IFileService fileService)
        {
            _fileService = fileService;
            _categories = _fileService.Read<string>(RepositoryPaths.Categories);

            // İlk işə salındıqda default kateqoriyalar
            if (_categories.Count == 0)
            {
                _categories = new List<string>
                {
                    "Electronics",
                    "Clothing",
                    "Food & Beverage",
                    "Books",
                    "Sports",
                    "Home & Garden",
                    "Other"
                };
                SaveToFile();
            }
        }

        private void SaveToFile() => _fileService.Write(RepositoryPaths.Categories, _categories);

        public IReadOnlyList<string> GetAll() => _categories.AsReadOnly();

        // ── Seçim (CreateProduct / UpdateProduct üçün) ─────────────────────────
        /// <summary>
        /// Kateqoriya siyahısını göstərir, seçilməsini gözləyir.
        /// Uğursuzluqda null qaytarır.
        /// </summary>
        public string? SelectCategory()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n  ── Mövcud Kateqoriyalar ──");
            Console.ResetColor();

            for (int i = 0; i < _categories.Count; i++)
                Console.WriteLine($"  {i + 1,2}. {_categories[i]}");

            Console.Write("Kateqoriya seç (nömrə): ");
            if (!int.TryParse(Console.ReadLine(), out int choice) ||
                choice < 1 || choice > _categories.Count)
            {
                PrintError("[Error] Yanlış seçim.");
                return null;
            }
            return _categories[choice - 1];
        }

        // ── Kateqoriya idarəetmə menyusu ───────────────────────────────────────
        public void ManageCategories()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("──── Category Management ────\n");
                Console.ResetColor();

                Console.WriteLine("Mövcud kateqoriyalar:");
                for (int i = 0; i < _categories.Count; i++)
                    Console.WriteLine($"  {i + 1,2}. {_categories[i]}");

                Console.WriteLine();
                Console.WriteLine("  1. Kateqoriya əlavə et");
                Console.WriteLine("  2. Kateqoriya sil");
                Console.WriteLine("  0. Geri");
                Console.Write("\nSeçim: ");

                string choice = Console.ReadLine()?.Trim() ?? "";
                switch (choice)
                {
                    case "1": AddCategory(); break;
                    case "2": DeleteCategory(); break;
                    case "0": return;
                    default: PrintError("[Error] Yanlış seçim."); break;
                }

                if (choice != "0")
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\nDavam etmək üçün bir düymə basın...");
                    Console.ResetColor();
                    Console.ReadKey(intercept: true);
                }
            }
        }

        // ── Əlavə et ───────────────────────────────────────────────────────────
        private void AddCategory()
        {
            Console.WriteLine("\n──── Add Category ────");
            Console.Write("Yeni kateqoriya adı: ");
            string name = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(name))
            {
                PrintError("[Error] Ad boş ola bilməz.");
                return;
            }
            if (_categories.Any(c => c.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                PrintError($"[Error] '{name}' artıq mövcuddur.");
                return;
            }

            _categories.Add(name);
            SaveToFile();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] '{name}' kateqoriyası əlavə edildi.");
            Console.ResetColor();
        }

        // ── Sil ────────────────────────────────────────────────────────────────
        private void DeleteCategory()
        {
            if (_categories.Count == 0) { PrintError("[Error] Kateqoriya yoxdur."); return; }

            Console.WriteLine("\n──── Delete Category ────");
            for (int i = 0; i < _categories.Count; i++)
                Console.WriteLine($"  {i + 1,2}. {_categories[i]}");

            Console.Write("Silinəcək kateqoriyanın nömrəsi: ");
            if (!int.TryParse(Console.ReadLine(), out int choice) ||
                choice < 1 || choice > _categories.Count)
            {
                PrintError("[Error] Yanlış seçim.");
                return;
            }

            string cat = _categories[choice - 1];
            Console.Write($"'{cat}' silinsin? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() != "y")
            {
                Console.WriteLine("[Info] Ləğv edildi.");
                return;
            }

            _categories.RemoveAt(choice - 1);
            SaveToFile();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] '{cat}' silindi.");
            Console.ResetColor();
        }

        // ── Köməkçi ────────────────────────────────────────────────────────────
        private static void PrintError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
