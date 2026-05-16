using Mini_Project1.Display;
using Mini_Project1.Interfaces;
using Mini_Project1.Models;
using Mini_Project1.Repository;
using Mini_Project1.Helpers;
using Mini_Project1.Factories;


namespace Mini_Project1.Services
{
    internal class OrderServices
    {
        private readonly IFileService _fileService;
        private readonly ProductServices _productService;
        private List<Order> _orders;

        public OrderServices(IFileService fileService, ProductServices productService)
        {
            _fileService = fileService;
            _productService = productService;
            _orders = _fileService.Read<Order>(RepositoryPaths.Orders);
        }

        private void SaveToFile()
            => _fileService.Write(RepositoryPaths.Orders, _orders);

        private Order? FindOrderById(Guid id)
            => _orders.FirstOrDefault(o => o.Id == id);

        // ══════════════════════════════════════════════
        //  6. Sifariş ver
        // ══════════════════════════════════════════════

        public void OrderProduct()
        {
            Console.WriteLine("\n──── Order Product ────");

            if (!ConsoleInputHelper.TryReadEmail(out string email)) return;

            var orderItems = new List<OrderItem>();

            while (true)
            {
                bool selected = ConsoleInputHelper.TryReadProduct(
                    _productService, out var product, out int count);

                if (!selected)
                {
                    if (product == null) break; // Guid.Empty — döngüdən çıx
                    continue;                   // Validation xətası — yenidən cəhd et
                }

                var item = OrderFactory.CreateItem(product!, count);
                orderItems.Add(item);
                Console.WriteLine($"[OK] Added: {product!.Name} x{count} = ${item.SubTotal:F2}");

                Console.Write("Add another product? (y/n): ");
                if (Console.ReadLine()?.Trim().ToLower() != "y") break;
            }

            if (orderItems.Count == 0)
            {
                Console.WriteLine("[Info] Order cancelled. No items selected.");
                return;
            }

            var order = OrderFactory.CreateOrder(email, orderItems);

            foreach (var item in order.Items)
                _productService.UpdateProductStock(item.Product, item.ProductCount);

            _orders.Add(order);
            SaveToFile();

            Console.WriteLine("\n[OK] Order placed successfully!");
            ConsoleRenderer.PrintOrder(order);
        }

        // ══════════════════════════════════════════════
        //  7. Bütün sifarişlər
        // ══════════════════════════════════════════════

        public void ShowAllOrders()
        {
            Console.WriteLine("\n──── All Orders ────");

            if (_orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
                return;
            }

            foreach (var order in _orders)
            {
                Console.WriteLine("\n  ══════════════════════════════");
                ConsoleRenderer.PrintOrder(order);
            }

            Console.WriteLine($"\n  ══════════════════════════════");
            Console.WriteLine($"Total: {_orders.Count} order(s)");
        }

        // ══════════════════════════════════════════════
        //  8. Sifariş statusunu dəyiş
        // ══════════════════════════════════════════════

        public void ChangeOrderStatus()
        {
            Console.WriteLine("\n──── Change Order Status ────");

            if (!ConsoleInputHelper.TryReadOrderStatus(out var newStatus, out var id)) return;

            var order = FindOrderById(id);
            if (order == null)
            {
                Console.WriteLine("[Error] Order not found.");
                return;
            }

            Console.WriteLine($"\nCurrent Status: {order.Status}");
            order.Status = newStatus;
            SaveToFile();

            Console.WriteLine($"[OK] Order #{id} status changed to '{newStatus}'.");
        }
        ///// Yaddaşdakı sifariş siyahısı. Tətbiq başlayanda fayldan yüklənir.

        //private List<Order> _orders;

        ///// <summary>
        ///// Məhsul məlumatlarına daxil olmaq üçün ProductService referansı.
        ///// Dependency Injection nümunəsi: servis konstruktorda ötürülür.
        ///// </summary>
        //private readonly ProductServices _productService;

        //// ──────────────────────────────────────────────
        ////  Constructor
        //// ──────────────────────────────────────────────

        //public OrderServices(ProductServices productService)
        //{
        //    _productService = productService;
        //    _orders = FileServices.Read<Order>(FileServices.OrdersFile);
        //}

        //// ──────────────────────────────────────────────
        ////  Köməkçi (Helper) metodlar
        //// ──────────────────────────────────────────────

        ///// <summary>
        ///// Yeni sifariş üçün unikal Id hesablayır.
        ///// </summary>
        //private Guid GetNextOrderId()
        //{
        //    return Guid.NewGuid();
        //}


        //// Yeni OrderItem üçün unikal Id hesablayır.
        //// Bütün sifarişlərdəki bütün item-ların max Id-sindən 1 çox olur.



        ///// <summary>
        ///// Sifariş siyahısını JSON faylına yazır.
        ///// </summary>
        //private void SaveToFile()
        //{
        //    FileServices.Write(FileServices.OrdersFile, _orders);
        //}

        //// ══════════════════════════════════════════════
        ////  6. Sifariş ver (Order Product)
        //// ══════════════════════════════════════════════

        ///// <summary>
        ///// Tam sifariş axışını idarə edir:
        /////   1. Email alınır (@ yoxlaması)
        /////   2. Döngüdə məhsul seçilir → say alınır → OrderItem yaranır
        /////   3. Məhsul seçimi bitdikdə Order yaranır, fayla yazılır
        /////   4. Seçilən məhsulların stoku azaldılır
        ///// </summary>
        //public void OrderProduct()
        //{
        //    Console.WriteLine("\n──── Order Product ────");

        //    // ── Email ──
        //    Console.Write("Your Email: ");
        //    string email = Console.ReadLine()?.Trim() ?? string.Empty;

        //    // Email @ xarakteri yoxlaması
        //    if (!email.Contains('@'))
        //    {
        //        Console.WriteLine("[Error] Email must contain '@' character.");
        //        return;
        //    }

        //    // Seçilmiş OrderItem-ləri saxlayan müvəqqəti siyahı
        //    var orderItems = new List<OrderItem>();

        //    // ── Məhsul seçim döngüsü ──
        //    while (true)
        //    {
        //        Console.Write("\nProduct ID (0 to finish): ");

        //        if (!Guid.TryParse(Console.ReadLine(), out Guid productId))
        //        {
        //            Console.WriteLine("[Error] Invalid ID.");
        //            continue;   // Döngünü davam etdir
        //        }

        //        // 0 daxil edildikdə sifariş tamamlanır
        //        if (productId == Guid.Empty)
        //        {
        //            // Heç məhsul seçilməyibsə sifarişi ləğv edirik
        //            if (orderItems.Count == 0)
        //            {
        //                Console.WriteLine("[Info] No items selected. Order cancelled.");
        //                return;
        //            }
        //            break;  // Məhsul seçim döngüsündən çıx
        //        }

        //        // Məhsul mövcuddurmu?
        //        var product = _productService.FindById(productId);
        //        if (product == null)
        //        {
        //            Console.WriteLine("[Error] Product not found.");
        //            continue;
        //        }

        //        // Məhsul stokda varmı?
        //        if (product.Stock == 0)
        //        {
        //            Console.WriteLine($"[Error] '{product.Name}' is out of stock.");
        //            continue;
        //        }

        //        // Say ──
        //        Console.Write($"How many '{product.Name}' (available: {product.Stock})? ");

        //        if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
        //        {
        //            Console.WriteLine("[Error] Count must be a positive number.");
        //            continue;
        //        }

        //        // İstənilən say stokdan çoxdursa sifariş keçmir
        //        if (count > product.Stock)
        //        {
        //            Console.WriteLine($"[Error] Not enough stock. Available: {product.Stock}");
        //            continue;
        //        }

        //        // ── OrderItem yarat ──
        //        var item = new OrderItem
        //        {
        //            Id = Guid.NewGuid(),  // Yeni unikal Id
        //            Product = product,
        //            ProductCount = count,
        //            Price = product.Price,                        // Qiymət snapshot
        //            SubTotal = product.Price * count                 // Hesablanmış ara məbləğ
        //        };

        //        orderItems.Add(item);
        //        Console.WriteLine($"[OK] Added: {product.Name} x{count} = ${item.SubTotal:F2}");

        //        // Başqa məhsul əlavə etmək istəyirmi?
        //        Console.Write("Add another product? (y/n): ");
        //        string? ans = Console.ReadLine()?.Trim().ToLower();
        //        if (ans != "y") break;
        //    }

        //    // Heç məhsul seçilməmişsə çıxış
        //    if (orderItems.Count == 0)
        //    {
        //        Console.WriteLine("[Info] Order cancelled. No items.");
        //        return;
        //    }

        //    // ── Order yarat ──
        //    var order = new Order
        //    {
        //        Id = GetNextOrderId(),
        //        Items = orderItems,
        //        Total = orderItems.Sum(i => i.SubTotal),  // Bütün SubTotal-ların cəmi
        //        Email = email,
        //        Status = OrderStatus.Pending,               // Default status
        //        OrderedAt = DateTime.Now
        //    };

        //    // Id-ləri son vəziyyətə görə düzgün set et
        //    Guid baseItemId = Guid.NewGuid();
        //    for (int i = 0; i < order.Items.Count; i++)
        //        order.Items[i].Id = Guid.NewGuid(); 

        //    // Stokları azalt (hər məhsul üçün)
        //    foreach (var item in order.Items)
        //        _productService.UpdateProductStock(item.Product, item.ProductCount);

        //    _orders.Add(order);
        //    SaveToFile();

        //    Console.WriteLine("\n[OK] Order placed successfully!");
        //    order.PrintInfo();
        //}

        //// ══════════════════════════════════════════════
        ////  7. Bütün sifarişlər (Show All Orders)
        //// ══════════════════════════════════════════════

        ///// <summary>
        ///// Bütün sifarişlərin məlumatlarını (status daxil) konsola çap edir.
        ///// </summary>
        //public void ShowAllOrders()
        //{
        //    // Fayl dəyişmiş ola bilər – hər dəfə fayldan yenidən yüklə
        //    _orders = FileServices.Read<Order>(FileServices.OrdersFile);

        //    Console.WriteLine("\n──── All Orders ────");

        //    if (_orders.Count == 0)
        //    {
        //        Console.WriteLine("No orders found.");
        //        return;
        //    }

        //    foreach (var order in _orders)
        //    {
        //        Console.WriteLine("\n  ══════════════════════════════");
        //        order.PrintInfo();
        //    }

        //    Console.WriteLine("\n  ══════════════════════════════");
        //    Console.WriteLine($"Total: {_orders.Count} order(s)");
        //}

        //// ══════════════════════════════════════════════
        ////  8. Sifariş statusunu dəyiş (Change Order Status)
        //// ══════════════════════════════════════════════



        //// mövcud status seçimlərini göstərir və seçilən statusu tətbiq edir.

        //public void ChangeOrderStatus()
        //{
        //    _orders = FileServices.Read<Order>(FileServices.OrdersFile);

        //    Console.WriteLine("\n──── Change Order Status ────");
        //    Console.Write("Order ID: ");

        //    if (!Guid.TryParse(Console.ReadLine(), out Guid id))
        //    {
        //        Console.WriteLine("[Error] Invalid ID format.");
        //        return;
        //    }

        //    var order = _orders.FirstOrDefault(o => o.Id == id);

        //    if (order == null)
        //    {
        //        Console.WriteLine("[Error] Order not found.");
        //        return;
        //    }

        //    Console.WriteLine($"\nCurrent Status: {order.Status}");
        //    Console.WriteLine("Select new status:");

        //    // Enum-un bütün dəyərlərini dinamik sıralayırıq
        //    // Bu sayədə enum-a yeni dəyər əlavə olsa kod dəyişməz
        //    var statuses = Enum.GetValues<OrderStatus>();
        //    for (int i = 0; i < statuses.Length; i++)
        //        Console.WriteLine($"  {i + 1}. {statuses[i]}");

        //    Console.Write("Choice: ");

        //    if (!int.TryParse(Console.ReadLine(), out int choice) ||
        //        choice < 1 || choice > statuses.Length)
        //    {
        //        Console.WriteLine("[Error] Invalid choice.");
        //        return;
        //    }

        //    // 1-əsaslı seçimi 0-əsaslı indeksə çevir
        //    OrderStatus newStatus = statuses[choice - 1];
        //    order.Status = newStatus;

        //    SaveToFile();

        //    Console.WriteLine($"[OK] Order #{id} status changed to '{newStatus}'.");
        //}
    }

}
