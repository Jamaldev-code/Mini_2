using Mini_Project1.Models;

namespace Mini_Project1.Services
{
    internal class ProductServices
    {


        // Yaddaşdakı məhsul siyahısı.
        // Tətbiq başlayanda fayldan yüklənir.
        
        private List<Product> _products;

        // ──────────────────────────────────────────────
        //  Constructor
        // ──────────────────────────────────────────────

        /// <summary>
        /// Servis yarananda mövcud məhsulları fayldan oxuyur.
        /// </summary>
        public ProductServices()
        {
            // Fayl varsa mövcud məhsulları yüklə, yoxdursa boş siyahı başlat
            _products = FileServices.Read<Product>(FileServices.ProductsFile);
        }

        // ──────────────────────────────────────────────
        //  Köməkçi (Helper) metodlar
        // ──────────────────────────────────────────────

        /// <summary>
        /// Yeni məhsul üçün unikal Id hesablayır.
        /// Cari siyahıdakı max Id-dən 1 çox olur.
        /// Siyahı boşdursa 1-dən başlayır.
        /// </summary>
        private Guid GetNextId()
        {
           return Guid.NewGuid();
        }

        /// <summary>
        /// Məhsul siyahısını JSON faylına yazır (yaddaşdakı vəziyyəti saxlayır).
        /// </summary>
        private void SaveToFile()
        {
            FileServices.Write(FileServices.ProductsFile, _products);
        }

        /// <summary>
        /// Id-yə görə məhsul axtarır. Tapılmasa null qaytarır.
        /// Bu metod xarici servislərdən (OrderService) də çağırılır.
        /// </summary>
        public Product? FindById(Guid id)
        {
            // LINQ FirstOrDefault: şərtə uyan ilk elementi qaytarır, yoxdursa null
            return _products.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Məhsulun stokunu dəyişdirdikdən sonra faylı yeniləmək üçün çağırılır.
        /// OrderService tərəfindən sifariş zamanı istifadə olunur.
        /// </summary>
        public void UpdateProductStock(Product product, int soldCount)
        {
            // Stokdan satılan miqdarı çıxırıq
            product.Stock -= soldCount;
            SaveToFile();
        }

        // ══════════════════════════════════════════════
        //  1. Məhsul yarat (Create Product)
        // ══════════════════════════════════════════════

        /// <summary>
        /// İstifadəçidən Name, Price, Stock alır, doğrulayır və məhsul yaradır.
        /// Doğrulama qaydaları:
        ///   • Name ən azı 1 xarakter olmalıdır
        ///   • Eyni adlı başqa məhsul olmamalıdır
        ///   • Price 0-dan böyük olmalıdır
        ///   • Stock mənfi olmamalıdır
        /// </summary>
        public void CreateProduct()
        {
            Console.WriteLine("\n──── Create Product ────");

            // ── Ad ──
            Console.Write("Product Name: ");
            string name = Console.ReadLine()?.Trim() ?? string.Empty;

            // Boş ad yoxlaması
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("[Error] Product name cannot be empty.");
                return;
            }

            // Unikallıq yoxlaması – case-insensitive müqayisə
            bool nameExists = _products.Any(p =>
                p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (nameExists)
            {
                Console.WriteLine($"[Error] A product named '{name}' already exists.");
                return;
            }

            // ── Qiymət ──
            Console.Write("Price: $");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
            {
                Console.WriteLine("[Error] Price must be a number greater than 0.");
                return;
            }

            // ── Stok ──
            Console.Write("Stock: ");
            if (!int.TryParse(Console.ReadLine(), out int stock) || stock < 0)
            {
                Console.WriteLine("[Error] Stock cannot be negative.");
                return;
            }

            // ── Məhsul yarat ──
            var product = new Product
            {
                //Id = GetNextId(),   
                Name = name,
                Price = price,
                Stock = stock
            };

            _products.Add(product);   // Yaddaş siyahısına əlavə et
            SaveToFile();             // Fayla yaz

            Console.WriteLine($"\n[OK] Product created successfully.");
            product.PrintInfo();
        }

        // ══════════════════════════════════════════════
        //  2. Məhsul sil (Delete Product)
        // ══════════════════════════════════════════════

        /// <summary>
        /// İstifadəçidən Id alır, həmin Id-li məhsulu tapıb silir.
        /// </summary>
        public void DeleteProduct()
        {
            Console.WriteLine("\n──── Delete Product ────");
            Console.Write("Product ID to delete: ");

            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("[Error] Invalid ID format.");
                return;
            }

            var product = FindById(id);

            if (product == null)
            {
                Console.WriteLine("[Error] Product not found.");
                return;
            }

            _products.Remove(product);   // Yaddaşdan sil
            SaveToFile();                // Faylı yenilə

            Console.WriteLine($"[OK] '{product.Name}' (ID: {id}) deleted successfully.");
        }

        // ══════════════════════════════════════════════
        //  3. Id ilə məhsul axtar (Get Product By Id)
        // ══════════════════════════════════════════════

        /// <summary>
        /// İstifadəçidən Id alır, tapılırsa məhsul məlumatlarını göstərir.
        /// </summary>
        public void GetProductById()
        {
            Console.WriteLine("\n──── Get Product By ID ────");
            Console.Write("Product ID: ");

            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("[Error] Invalid ID format.");
                return;
            }

            var product = FindById(id);

            if (product == null)
            {
                Console.WriteLine("[Error] Product not found.");
                return;
            }

            Console.WriteLine();
            product.PrintInfo();
        }

        // ══════════════════════════════════════════════
        //  4. Bütün məhsulları göstər (Show All Products)
        // ══════════════════════════════════════════════

        /// <summary>
        /// Bütün məhsulları sıra ilə çap edir.
        /// Stoqu bitmiş məhsulların yanında "Out of Stock" yazılır.
        /// </summary>
        public void ShowAllProducts()
        {
            Console.WriteLine("\n──── All Products ────");

            if (_products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }

            // Hər məhsulu ayırıcı xəttlə çap edirik
            foreach (var product in _products)
            {
                Console.WriteLine("  ─────────────────────");
                product.PrintInfo();
            }

            Console.WriteLine("  ─────────────────────");
            Console.WriteLine($"Total: {_products.Count} product(s)");
        }

        // ══════════════════════════════════════════════
        //  5. Stok doldur (Refill Product)
        // ══════════════════════════════════════════════

        /// <summary>
        /// İstifadəçidən Id alır, məhsul tapılırsa əlavə ediləcək stok miqdarını
        /// soruşur və mövcud stoka əlavə edir.
        /// </summary>
        public void RefillProduct()
        {
            Console.WriteLine("\n──── Refill Product ────");
            Console.Write("Product ID: ");

            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("[Error] Invalid ID format.");
                return;
            }

            var product = FindById(id);

            if (product == null)
            {
                // Tapılmasa mesaj yazıb çıxırıq (ana menüyə qayıdılır)
                Console.WriteLine("Product not found");
                return;
            }

            Console.Write($"Add stock amount for '{product.Name}' (current: {product.Stock}): ");

            if (!int.TryParse(Console.ReadLine(), out int amount) || amount < 0)
            {
                Console.WriteLine("[Error] Amount cannot be negative.");
                return;
            }

            product.Stock += amount;   // Stok artırılır
            SaveToFile();

            Console.WriteLine($"[OK] Stock updated. New stock: {product.Stock}");
        }
    }
}
