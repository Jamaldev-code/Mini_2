using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project1.Models
{
    internal class Order
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public List<OrderItem> Items { get; set; } = new();

        
        // Bütün OrderItem-ların SubTotal cəmi.
        // Sifariş yarananda hesablanıb saxlanır.
        
        public decimal Total { get; set; }

        public string Email { get; set; } = string.Empty;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime OrderedAt { get; set; }

        public void PrintInfo()
        {
            Console.WriteLine($"  Order ID  : {Id}");
            Console.WriteLine($"  Email     : {Email}");
            Console.WriteLine($"  Status    : {Status}");
            Console.WriteLine($"  Ordered At: {OrderedAt:dd.MM.yyyy HH:mm}");
            Console.WriteLine("  Items:");

            // Hər OrderItem-i sıra nömrəsi ilə çap edirik
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                Console.WriteLine($"    {i + 1}) {item.Product.Name} x{item.ProductCount}" +
                                  $"  @${item.Price:F2}  => SubTotal: ${item.SubTotal:F2}");
            }

            Console.WriteLine($"  ─────────────────────────────");
            Console.WriteLine($"  Total     : ${Total:F2}");
        }
    }
}
