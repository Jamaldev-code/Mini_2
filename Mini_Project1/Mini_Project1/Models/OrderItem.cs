

namespace Mini_Project1.Models
{
    internal class OrderItem
    {
  
        public Guid Id { get; set; }

        public Product Product { get; set; } = null!;

        public int ProductCount { get; set; }

        public decimal Price { get; set; }

        public decimal SubTotal { get; set; }
    }
}
