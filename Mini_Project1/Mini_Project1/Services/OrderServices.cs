using Mini_Project1.Abstractions;
using Mini_Project1.Constants;
using Mini_Project1.Display;
using Mini_Project1.Factories;
using Mini_Project1.Helpers;
using Mini_Project1.Models;
using Mini_Project1.UI;

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

        private void SaveToFile() => _fileService.Write(RepositoryPaths.Orders, _orders);
        private Order? FindOrderById(Guid id) => _orders.FirstOrDefault(o => o.Id == id);

        private void RestoreStock(Order order)
        {
            foreach (var item in order.Items)
            {
                _productService.RestoreStock(item.Product, item.ProductCount);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"  [Stok] '{item.Product.Name}' +{item.ProductCount} stoka qayitdi => yeni stok: {item.Product.Stock}");
                Console.ResetColor();
            }
        }

        // 8. Sifaris ver
        public void OrderProduct()
        {
            Console.WriteLine("\n---- Order Product ----");

            // FIX: ReadWithRetry - 2 yanlish cemddhen sonra ana menuya qayitmag sorusulur
            string email = "";
            if (!ConsoleInputHelper.ReadWithRetry(() => ConsoleInputHelper.TryReadEmail(out email))) return;

            string phone = "";
            if (!ConsoleInputHelper.ReadWithRetry(() => ConsoleInputHelper.TryReadPhone(out phone))) return;

            var orderItems = new List<OrderItem>();
            int prodFails = 0;

            while (true)
            {
                bool selected = ConsoleInputHelper.TryReadProduct(_productService, out var product, out int count);

                if (!selected)
                {
                    if (product == null) break;
                    if (++prodFails >= 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("\nAna menuya qayitmaq isteyirsiniz? (y/n): ");
                        Console.ResetColor();
                        if (Console.ReadLine()?.Trim().ToLower() == "y") return;
                        prodFails = 0;
                    }
                    continue;
                }

                prodFails = 0;
                _productService.UpdateProductStock(product!, count);

                // FIX: Eyni mehsul yeniden secilende birleshdirmek
                var existing = orderItems.FirstOrDefault(i => i.Product.Id == product!.Id);
                if (existing != null)
                {
                    existing.ProductCount += count;
                    existing.SubTotal += product!.Price * count;
                    Console.WriteLine($"[OK] '{product!.Name}' miqdari yenilendi: x{existing.ProductCount} = ${existing.SubTotal:F2}");
                }
                else
                {
                    var item = OrderFactory.CreateItem(product!, count);
                    orderItems.Add(item);
                    Console.WriteLine($"[OK] Elave edildi: {product!.Name} x{count} = ${item.SubTotal:F2}");
                }

                if (product!.Stock <= 5)
                    AlertUI.ShowLowStockNotification(product);

                Console.Write("Basqa mehsul secmek isteyirsiniz? (y/n): ");
                if (Console.ReadLine()?.Trim().ToLower() != "y") break;
            }

            if (orderItems.Count == 0)
            {
                Console.WriteLine("[Melumat] Sifaris legv edildi. Mehsul secilmedi.");
                return;
            }

            // Total yeniden hesabla (birleshdirme sonrasi)
            Order order = OrderFactory.CreateOrder(email, phone, orderItems);
            _orders.Add(order);
            SaveToFile();

            Console.WriteLine("\n[OK] Sifaris ugurla yaradildi!");
            ConsoleRenderer.PrintOrder(order);
        }

        // 9. Butun sifarishler - FIX: yeni dizayn
        public void ShowAllOrders()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---- All Orders ----\n");
            Console.ResetColor();

            if (_orders.Count == 0)
            {
                Console.WriteLine("Hec bir sifaris tapilmadi.");
                return;
            }

            for (int i = 0; i < _orders.Count; i++)
            {
                ConsoleRenderer.PrintOrder(_orders[i], i + 1);
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Cemisi: {_orders.Count} sifaris");
            Console.ResetColor();
        }

        // 10. Sifaris statusunu deyish
        public void ChangeOrderStatus()
        {
            Console.WriteLine("\n---- Change Order Status ----");
            if (!ConsoleInputHelper.ReadWithRetry(
                () => ConsoleInputHelper.TryReadOrderStatus(out var st, out var tid) && TryApplyStatus(st, tid))) return;
        }

        private bool TryApplyStatus(OrderStatus newStatus, Guid id)
        {
            var order = FindOrderById(id);
            if (order == null) { PrintError("[Xeta] Sifaris tapilmadi."); return false; }
            if (order.Status == OrderStatus.Cancelled) { PrintError("[Xeta] Legv edilmish sifarishin statusu deyishdirilmez."); return false; }

            Console.WriteLine($"\nMovcud status: {order.Status}");
            if (newStatus == OrderStatus.Cancelled)
            {
                Console.WriteLine("[Stok] Mehsullar stoka qaytarilir...");
                RestoreStock(order);
            }
            order.Status = newStatus;
            SaveToFile();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] Sifaris #{id} statusu '{newStatus}' oldu.");
            Console.ResetColor();
            return true;
        }

        // 11. Email ile sifaris axtar
        public void SearchOrdersByEmail()
        {
            Console.WriteLine("\n---- Search Orders by Email ----");
            string email = "";
            if (!ConsoleInputHelper.ReadWithRetry(() => ConsoleInputHelper.TryReadEmail(out email))) return;

            var results = _orders
                .Where(o => o.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (results.Count == 0) { Console.WriteLine($"[Melumat] '{email}' ucun sifaris tapilmadi."); return; }

            Console.WriteLine($"\n'{email}' ucun {results.Count} sifaris tapildi:");
            for (int i = 0; i < results.Count; i++)
            {
                ConsoleRenderer.PrintOrder(results[i], i + 1);
                Console.WriteLine();
            }
        }

        // 12. Sifarishi legv et
        public void CancelOrder()
        {
            Console.WriteLine("\n---- Cancel Order ----");
            Console.Write("Sifaris ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id)) { PrintError("[Xeta] Yanlish ID."); return; }

            var order = FindOrderById(id);
            if (order == null) { PrintError("[Xeta] Tapilmadi."); return; }
            if (order.Status == OrderStatus.Cancelled) { PrintError("[Xeta] Artiq legv edilmishdir."); return; }
            if (order.Status == OrderStatus.Completed) { PrintError("[Xeta] Tamamlanmish sifaris legv edilemez."); return; }

            ConsoleRenderer.PrintOrder(order);
            Console.Write("\nBu sifarishi legv etmek isteyirsiniz? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() != "y") { Console.WriteLine("[Melumat] Emeliyyat legv edildi."); return; }

            Console.WriteLine("[Stok] Mehsullar stoka qaytarilir...");
            RestoreStock(order);
            order.Status = OrderStatus.Cancelled;
            SaveToFile();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] Sifaris #{id} legv edildi.");
            Console.ResetColor();
        }

        // 13. Statistika
        public void ShowProductStats()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---- Product Statistics ----\n");
            Console.ResetColor();

            var active = _orders.Where(o => o.Status != OrderStatus.Cancelled).ToList();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("En cox satilan mehsullar (Top 10):");
            Console.ResetColor();
            Console.WriteLine("  " + new string('-', 65));

            var best = active.SelectMany(o => o.Items)
                .GroupBy(i => i.Product.Id)
                .Select(g => new { Name = g.First().Product.Name, Cat = g.First().Product.Category, Sold = g.Sum(i => i.ProductCount), Rev = g.Sum(i => i.SubTotal) })
                .OrderByDescending(x => x.Sold).Take(10).ToList();

            if (best.Count == 0) Console.WriteLine("  Hele hec bir sifaris yoxdur.");
            else
            {
                Console.WriteLine($"  {"#",-3} {"Mehsul",-22} {"Kateqoriya",-14} {"Satis",6} {"Gelir",12}");
                Console.WriteLine("  " + new string('-', 65));
                for (int i = 0; i < best.Count; i++)
                    Console.WriteLine($"  {i + 1,-3} {best[i].Name,-22} {best[i].Cat,-14} {best[i].Sold,6}  ${best[i].Rev,10:F2}");
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Son sifarishler (en son 5):");
            Console.ResetColor();

            var recent = _orders.OrderByDescending(o => o.OrderedAt).Take(5).ToList();
            if (recent.Count == 0) Console.WriteLine("  Hele hec bir sifaris yoxdur.");
            else
                foreach (var o in recent)
                {
                    string icon = o.Status switch { OrderStatus.Cancelled => "[X]", OrderStatus.Completed => "[v]", OrderStatus.Confirmed => "[>]", _ => "[?]" };
                    Console.WriteLine($"  {icon} [{o.OrderedAt:dd.MM.yyyy HH:mm}]  {o.Status,-12}  #{o.Id.ToString()[..8]}...");
                    foreach (var item in o.Items)
                        Console.WriteLine($"    +-- {item.Product.Name,-22} x{item.ProductCount,3}  @${item.Price:F2}");
                    Console.WriteLine($"    Umumi: ${o.Total:F2}\n");
                }

            Console.WriteLine("  " + new string('-', 65));
            Console.WriteLine($"  Umumi sifaris     : {_orders.Count}");
            Console.WriteLine($"  Aktiv             : {active.Count}");
            Console.WriteLine($"  Legv edilmish     : {_orders.Count(o => o.Status == OrderStatus.Cancelled)}");
            Console.WriteLine($"  Tamamlanmish      : {_orders.Count(o => o.Status == OrderStatus.Completed)}");
            if (active.Count > 0)
                Console.WriteLine($"  Umumi gelir       : ${active.Sum(o => o.Total):F2}");
        }

        private static void PrintError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
