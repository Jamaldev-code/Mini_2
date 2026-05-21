using Mini_Project1.Abstractions;
using Mini_Project1.Constants;
using Mini_Project1.Display;
using Mini_Project1.Helpers;
using Mini_Project1.Models;
using System.Globalization;

namespace Mini_Project1.Services
{
    internal class ProductServices
    {
        private readonly IFileService _fileService;
        private readonly CategoryService _categoryService;
        private List<Product> _products;

        public ProductServices(IFileService fileService, CategoryService categoryService)
        {
            _fileService = fileService;
            _categoryService = categoryService;
            _products = _fileService.Read<Product>(RepositoryPaths.Products);
        }

        private void SaveToFile() => _fileService.Write(RepositoryPaths.Products, _products);
        public Product? FindById(Guid id) => _products.FirstOrDefault(p => p.Id == id);
        public IReadOnlyList<Product> GetAll() => _products.AsReadOnly();

        public void UpdateProductStock(Product product, int soldCount)
        {
            var e = FindById(product.Id);
            if (e == null) return;
            e.Stock = Math.Max(0, e.Stock - soldCount);
            product.Stock = e.Stock;
            SaveToFile();
        }

        public void RestoreStock(Product product, int quantity)
        {
            var e = FindById(product.Id);
            if (e == null) return;
            e.Stock += quantity;
            product.Stock = e.Stock;
            SaveToFile();
        }

        // 1. Mehsul yarat
        public void CreateProduct()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---- Create Product ----\n");
            Console.ResetColor();

            Console.Write("Mehsul adi: ");
            string name = Console.ReadLine()?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(name)) { PrintError("[Xeta] Ad bos ola bilmez."); return; }
            if (char.IsDigit(name[0])) { PrintError("[Xeta] Ad reqemle bashlaya bilmez."); return; }
            if (_products.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            { PrintError($"[Xeta] '{name}' artiq movcuddur."); return; }

            string? category = _categoryService.SelectCategory();
            if (category == null) return;

            // FIX: decimal - hem "." hem "," + retry
            decimal price = 0;
            if (!ConsoleInputHelper.ReadWithRetry(
                () => ConsoleInputHelper.TryReadDecimal("Qiymet: $", out price, 0.01m))) return;

            Console.Write("Stok miqdari: ");
            if (!int.TryParse(Console.ReadLine(), out int stock) || stock < 0)
            { PrintError("[Xeta] Stok menfi ola bilmez."); return; }

            var product = new Product { Name = name, Category = category, Price = Math.Round(price, 2), Stock = stock };
            _products.Add(product);
            SaveToFile();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[OK] Mehsul ugurla yaradildi.");
            ConsoleRenderer.PrintProduct(product);
            Console.ResetColor();
        }

        // 2. Mehsul sil
        public void DeleteProduct()
        {
            ProductPanelHelper.Draw(_products, "Delete Product");
            Console.Write("Silinecek mehsulun ID-si: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id)) { PrintError("[Xeta] Yanlish ID."); return; }
            var p = FindById(id);
            if (p == null) { PrintError("[Xeta] Tapilmadi."); return; }
            Console.Write($"'{p.Name}' silinsin? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() != "y") { Console.WriteLine("[Melumat] Legv edildi."); return; }
            _products.Remove(p);
            SaveToFile();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] '{p.Name}' silindi.");
            Console.ResetColor();
        }

        // 3. ID ile tap
        public void GetProductById()
        {
            ProductPanelHelper.Draw(_products, "Get Product By ID");
            Console.Write("Mehsul ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id)) { PrintError("[Xeta] Yanlish ID."); return; }
            var p = FindById(id);
            if (p == null) { PrintError("[Xeta] Tapilmadi."); return; }
            Console.WriteLine();
            ConsoleRenderer.PrintProduct(p);
        }

        // 4. Butun mehsullar - kateqoriye gore qruplanmish qapalı cercive
        public void ShowAllProducts()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---- All Products ----\n");
            Console.ResetColor();
            if (_products.Count == 0) { PrintError("Mehsul tapilmadi."); return; }

            var grouped = _products.OrderBy(p => p.Category).ThenBy(p => p.Name).GroupBy(p => p.Category);

            const int W_Name = 22;
            const int W_Price = 18;
            const int W_Stock = 5;
            const int W_Id = 36;
            const int IW = 2 + W_Name + 3 + W_Price + 3 + W_Stock + 3 + W_Id + 2; // = 94

            int totalShown = 0;
            foreach (var group in grouped)
            {
                string catLabel = $"  CATEGORY: {group.Key}  ";
                int fl = (IW - catLabel.Length) / 2;
                int fr = IW - catLabel.Length - fl;
                if (fl < 0) fl = 0; if (fr < 0) fr = 0;

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\u2554" + new string('\u2550', fl) + catLabel + new string('\u2550', fr) + "\u2557");
                Console.WriteLine("\u2560" + new string('\u2550', IW) + "\u2563");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"\u2551 {"Ad",-W_Name} \u2502 {"Qiymet",-W_Price} \u2502 {"Stok",W_Stock} \u2502 {"ID",-W_Id} \u2551");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\u2560" + new string('\u2550', IW) + "\u2563");

                var list = group.ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    var p = list[i];
                    string nm = p.Name.Length > W_Name ? p.Name[..(W_Name - 1)] + "." : p.Name;
                    string pr = p.HasDiscount ? $"${p.Price:F2} (-{p.DiscountPercent:F0}%)" : $"${p.Price:F2}";
                    string stk = p.Stock.ToString();

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write($"\u2551 ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{nm,-W_Name}");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(" \u2502 ");
                    Console.ForegroundColor = p.HasDiscount ? ConsoleColor.Green : ConsoleColor.Cyan;
                    Console.Write($"{pr,-W_Price}");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(" \u2502 ");
                    if (p.Stock == 0) Console.ForegroundColor = ConsoleColor.Red;
                    else if (p.Stock <= 5) Console.ForegroundColor = ConsoleColor.Yellow;
                    else Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{stk,W_Stock}");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(" \u2502 ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write($"{p.Id,-W_Id}");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine(" \u2551");

                    if (i < list.Count - 1)
                        Console.WriteLine("\u255f" + new string('\u2500', IW) + "\u2562");
                    totalShown++;
                }
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\u255a" + new string('\u2550', IW) + "\u255d");
                Console.ResetColor();
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Cemi: {totalShown} mehsul  |  {grouped.Count()} kateqoriya");
            Console.ResetColor();
        }

        // 5. Stok artir
        public void RefillProduct()
        {
            ProductPanelHelper.Draw(_products, "Refill Product");
            Console.Write("Mehsul ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id)) { PrintError("[Xeta] Yanlish ID."); return; }
            var p = FindById(id);
            if (p == null) { PrintError("[Xeta] Tapilmadi."); return; }
            Console.Write($"'{p.Name}' ucun stok elave et (movcud: {p.Stock}): ");
            if (!int.TryParse(Console.ReadLine(), out int amt) || amt < 0) { PrintError("[Xeta] Manfi ola bilmez."); return; }
            p.Stock += amt;
            SaveToFile();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] '{p.Name}' stok: {p.Stock}");
            Console.ResetColor();
        }

        // 6. Mehsulu yenile (endirim idarəetməsi daxil)
        public void UpdateProduct()
        {
            ProductPanelHelper.Draw(_products, "Update Product");
            Console.Write("Mehsul ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id)) { PrintError("[Xeta] Yanlish ID."); return; }
            var p = FindById(id);
            if (p == null) { PrintError("[Xeta] Tapilmadi."); return; }

            Console.WriteLine("\nBos buragsaniz, movcud deger saxlanilir.\n");

            // ── Ad ────────────────────────────────────────────────────────────
            Console.Write($"Ad [{p.Name}]: ");
            string newName = Console.ReadLine()?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(newName))
            {
                if (char.IsDigit(newName[0])) { PrintError("[Xeta] Ad reqemle bashlaya bilmez."); return; }
                if (_products.Any(x => x.Id != p.Id && x.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)))
                { PrintError($"[Xeta] '{newName}' artiq movcuddur."); return; }
                p.Name = newName;
            }

            // ── Kateqoriya ────────────────────────────────────────────────────
            Console.WriteLine($"Movcud kateqoriya: {p.Category}");
            Console.Write("Kateqoriyanı deyish? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() == "y")
            {
                string? newCat = _categoryService.SelectCategory();
                if (newCat == null) return;
                p.Category = newCat;
            }

            // ── Qiymət + Endirim (birləşdirilmiş) ────────────────────────────
            Console.WriteLine();
            if (p.HasDiscount)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  Movcud endirim   : {p.DiscountPercent:F0}%");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  Orijinal qiymet  : ${p.OriginalPrice:F2}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  Endirimli qiymet : ${p.Price:F2}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"  Movcud qiymet : ${p.Price:F2}  (endirim yoxdur)");
            }

            Console.WriteLine();
            Console.WriteLine("  Qiymet/endirim secenekleri:");
            Console.WriteLine("    1. Qiymeti deyish");
            Console.WriteLine("    2. Endirim tetbiq et / deyish");
            Console.WriteLine("    3. Endirimi legv et");
            Console.WriteLine("    0. Deyishiklik etme");
            Console.Write("  Secim: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1":
                    // Qiymeti deyish
                    decimal newPrice = 0;
                    if (!ConsoleInputHelper.ReadWithRetry(
                        () => ConsoleInputHelper.TryReadDecimal(
                            p.HasDiscount
                                ? $"Yeni orijinal qiymet (movcud: ${p.OriginalPrice:F2}): $"
                                : $"Yeni qiymet (movcud: ${p.Price:F2}): $",
                            out newPrice, 0.01m))) return;

                    if (p.HasDiscount)
                    {
                        p.OriginalPrice = Math.Round(newPrice, 2);
                        p.Price = Math.Round(p.OriginalPrice * (1 - p.DiscountPercent / 100m), 2);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"[OK] Yeni endirimli qiymet: ${p.Price:F2}");
                    }
                    else
                    {
                        p.Price = Math.Round(newPrice, 2);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"[OK] Qiymet yenilendi: ${p.Price:F2}");
                    }
                    Console.ResetColor();
                    break;

                case "2":
                    // Endirim tetbiq et
                    decimal discount = 0;
                    if (!ConsoleInputHelper.ReadWithRetry(
                        () => ConsoleInputHelper.TryReadDecimal("Endirim faizi (1-99): ", out discount, 0.01m))) return;
                    if (discount >= 100) { PrintError("[Xeta] Endirim 99%-den az olmalidir."); return; }

                    decimal basePrice = p.HasDiscount ? p.OriginalPrice : p.Price;
                    p.OriginalPrice = basePrice;
                    p.DiscountPercent = discount;
                    p.Price = Math.Round(basePrice * (1 - discount / 100m), 2);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[OK] {discount:F0}% endirim tetbiq edildi:");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"     Orijinal qiymet  : ${basePrice:F2}");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"     Endirimli qiymet : ${p.Price:F2}");
                    Console.ResetColor();
                    break;

                case "3":
                    // Endirimi legv et
                    if (!p.HasDiscount) { Console.WriteLine("[Melumat] Bu mehsulda endirim yoxdur."); break; }
                    decimal restored = p.OriginalPrice;
                    p.Price = restored;
                    p.OriginalPrice = 0;
                    p.DiscountPercent = 0;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[OK] Endirim legv edildi. Qiymet berpa olundu: ${p.Price:F2}");
                    Console.ResetColor();
                    break;

                default:
                    Console.WriteLine("[Melumat] Qiymet deyishdirilmedi.");
                    break;
            }

            SaveToFile();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[OK] Mehsul yenilendi.");
            ConsoleRenderer.PrintProduct(p);
            Console.ResetColor();
        }

        private static void PrintError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
