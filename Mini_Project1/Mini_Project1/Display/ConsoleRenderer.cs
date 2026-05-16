using Mini_Project1.Models;

namespace Mini_Project1.Display
{
    internal static class ConsoleRenderer
    {
        public static void PrintProduct(Product p)
        {
            string stockDisplay = p.Stock == 0 ? "Out of Stock" : p.Stock.ToString();

            Console.WriteLine($"  ID    : {p.Id}");
            Console.WriteLine($"  Name  : {p.Name}");
            Console.WriteLine($"  Price : ${p.Price:F2}");
            Console.WriteLine($"  Stock : {stockDisplay}");
        }

        public static void PrintOrder(Order o)
        {
            Console.WriteLine($"  Order ID  : {o.Id}");
            Console.WriteLine($"  Email     : {o.Email}");
            Console.WriteLine($"  Status    : {o.Status}");
            Console.WriteLine($"  Ordered At: {o.OrderedAt:dd.MM.yyyy HH:mm}");
            Console.WriteLine("  Items:");

            for (int i = 0; i < o.Items.Count; i++)
            {
                var item = o.Items[i];
                Console.WriteLine($"    {i + 1}) {item.Product.Name} x{item.ProductCount}" +
                                  $"  @${item.Price:F2}  => SubTotal: ${item.SubTotal:F2}");
            }

            Console.WriteLine($"  ─────────────────────────────");
            Console.WriteLine($"  Total     : ${o.Total:F2}");
        }
    }
}