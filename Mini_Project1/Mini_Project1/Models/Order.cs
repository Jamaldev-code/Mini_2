using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project1.Models
{
    internal class Order
    {
        private static int _id;

        public Guid Id { get; }
        public List<OrderItem> Items { get; set; }
        public decimal Total { get; set; }
        public string Email { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderedAt { get; set; }

        public Order(string email, List<OrderItem> items)
        {
            Id = Guid.NewGuid();
            Email = email;
            Items = items;

            Total = items.Sum(x => x.SubTotal);

            Status = OrderStatus.Pending;

            OrderedAt = DateTime.Now;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Order Id: {Id}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Status: {Status}");
            Console.WriteLine($"Total: {Total}");
            Console.WriteLine($"Ordered At: {OrderedAt}");

            foreach (OrderItem item in Items)
            {
                Console.WriteLine("----------------");
                Console.WriteLine($"Product: {item.Product.Name}");
                Console.WriteLine($"Count: {item.Count}");
                Console.WriteLine($"Price: {item.Price}");
                Console.WriteLine($"SubTotal: {item.SubTotal}");
            }
        }
    }
}
