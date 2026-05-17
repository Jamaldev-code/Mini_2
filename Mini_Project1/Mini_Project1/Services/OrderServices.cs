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
        private string phoneNumber;

        public OrderServices(IFileService fileService, ProductServices productService)
        {
            _fileService = fileService;
            _productService = productService;
            _orders = _fileService.Read<Order>(RepositoryPaths.Orders);
        }

        public OrderServices(ProductServices productService)
        {
            _productService = productService;
        }

        private void SaveToFile()
            => _fileService.Write(RepositoryPaths.Orders, _orders);

        private Order? FindOrderById(Guid id)
            => _orders.FirstOrDefault(o => o.Id == id);

       
        public void OrderProduct()
        {
            Console.WriteLine("\n──── Order Product ────");

            if (!ConsoleInputHelper.TryReadEmail(out string email)) return;
            if (!ConsoleInputHelper.TryReadPhone(out string phoneNumber)) return;
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

            Order order = OrderFactory.CreateOrder(email, phoneNumber, orderItems);

            foreach (var item in order.Items)
                _productService.UpdateProductStock(item.Product, item.ProductCount);

            _orders.Add(order);
            SaveToFile();

            Console.WriteLine("\n[OK] Order placed successfully!");
            ConsoleRenderer.PrintOrder(order);
        }

        //  7. Bütün sifarişlər

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

        //  8. Sifariş statusunu dəyiş


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
        
    }

}
