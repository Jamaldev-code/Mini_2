using Mini_Project1.Display;
using Mini_Project1.Interfaces;
using Mini_Project1.Models;
using Mini_Project1.Repository;

namespace Mini_Project1.Services
{
    internal class ProductServices
    {



        private readonly IFileService _fileService;
        private List<Product> _products;


        public ProductServices(IFileService fileService)
        {
            _fileService = fileService;
            _products = _fileService.Read<Product>(RepositoryPaths.Products);
        }

        private void SaveToFile()
        {
            _fileService.Write(RepositoryPaths.Products, _products);
        }


        // Id-yə görə məhsul axtarır. Tapılmasa null qaytarır.
        // Bu metod xarici servislərdən (OrderService) də çağırılır.

        public Product? FindById(Guid id)
        {
            // LINQ FirstOrDefault: şərtə uyan ilk elementi qaytarır, yoxdursa null
            return _products.FirstOrDefault(p => p.Id == id);
        }


        // Məhsulun stokunu dəyişdirdikdən sonra faylı yeniləmək üçün çağırılır.
        // OrderService tərəfindən sifariş zamanı istifadə olunur.

        public void UpdateProductStock(Product product, int soldCount)
        {
            // Stokdan satılan miqdarı çıxırıq
            product.Stock -= soldCount;
            SaveToFile();
        }


        //İstifadəçidən Name, Price, Stock alır, doğrulayır və məhsul yaradır.
        //Name ən azı 1 xarakter olmalıdır
        

        //Eyni adlı başqa məhsul olmamalıdır
        //Price 0-dan böyük olmalıdır
        //Stock mənfi olmamalıdır



        public void CreateProduct()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n──── Create Product ────");
            Console.ResetColor();

            // ── Ad ──
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Product Name: ");
            Console.ResetColor();
            string name = Console.ReadLine()?.Trim() ?? string.Empty;

            // Boş ad yoxlaması
            if (string.IsNullOrEmpty(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Product name cannot be empty.");
                Console.ResetColor();
                return;
            }

            // Name 1ci number qebul etmir
            if (char.IsDigit(name[0]))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Product name cannot start with a number.");
                Console.ResetColor();
                return;
            }


            // Unikallıq yoxlaması – case-insensitive müqayisə
            bool nameExists = _products.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (nameExists)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error] A product named '{name}' already exists.");
                Console.ResetColor();
                return;
            }

            // ── Qiymət ──
            Console.Write("Price: $");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Price cannot be 0 or negative and must contain only numbers.");
                Console.ResetColor();
                return;
            }

            // ── Stok ──

            Console.Write("Stock: ");
            if (!int.TryParse(Console.ReadLine(), out int stock) || stock < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Stock cannot be negative.");
                Console.ResetColor();
                return;
            }

            // ── Məhsul yarat ──
            var product = new Product
            {

                Name = name,
                Price = price,
                Stock = stock
            };

            _products.Add(product);   // Yaddaş siyahısına əlavə et
            SaveToFile();             // Fayla yaz
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[OK] Product created successfully.");
            ConsoleRenderer.PrintProduct(product);
            Console.ResetColor();
        }


        // İstifadəçidən Id alır, həmin Id-li məhsulu tapıb silir.

        public void DeleteProduct()
        {
            Console.WriteLine("\n──── Delete Product ────");
            Console.Write("Product ID to delete: ");

            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Invalid ID format.");
                Console.ResetColor();
                return;
            }

            var product = FindById(id);

            if (product == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Product not found.");
                return;
            }

            _products.Remove(product);   // Yaddaşdan sil
            SaveToFile();                // Faylı yenilə
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] '{product.Name}' (ID: {id}) deleted successfully.");
        }




        // İstifadəçidən Id alır, tapılırsa məhsul məlumatlarını göstərir.

        public void GetProductById()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n──── Get Product By ID ────");
            Console.Write("Product ID: ");

            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Invalid ID format.");
                return;
            }

            var product = FindById(id);

            if (product == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Product not found.");
                return;
            }

            Console.WriteLine();
            ConsoleRenderer.PrintProduct(product);
        }

        // Bütün məhsulları sıra ilə çap edir.
        // Stoqu bitmiş məhsulların yanında "Out of Stock" yazılır.

        public void ShowAllProducts()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n──── All Products ────");

            if (_products.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No products found.");
                return;
            }

            // Hər məhsulu ayırıcı xəttlə çap edirik
            foreach (var product in _products)
            {
                Console.WriteLine("  ─────────────────────");
                ConsoleRenderer.PrintProduct(product);
            }

            Console.WriteLine("  ─────────────────────");
            Console.WriteLine($"Total: {_products.Count} product(s)");
        }


        // İstifadəçidən Id alır, məhsul tapılırsa əlavə ediləcək stok miqdarını
        // soruşur və mövcud stoka əlavə edir.

        public void RefillProduct()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n──── Refill Product ────");
            Console.Write("Product ID: ");

            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Invalid ID format.");
                return;
            }

            var product = FindById(id);

            if (product == null)
            {
                // Tapılmasa mesaj yazıb çıxırıq (ana menüyə qayıdılır)
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error]Product not found");
                return;
            }

            Console.Write($"Add stock amount for '{product.Name}' (current: {product.Stock}): ");

            if (!int.TryParse(Console.ReadLine(), out int amount) || amount < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Amount cannot be negative.");
                return;
            }

            product.Stock += amount;   // Stok artırılır
            SaveToFile();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] Stock updated. New stock: {product.Stock}");
        }
    }
}
