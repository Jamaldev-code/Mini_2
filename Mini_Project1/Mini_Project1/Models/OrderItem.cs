

namespace Mini_Project1.Models
{
    internal class OrderItem
    {
        private static int _id;

        public Guid Id { get; }
        public Product Product { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }

        public OrderItem(Product product, int count)
        {
            Id = Guid.NewGuid();
            Product = product;
            Count = count;
            Price = product.Price;
            SubTotal = Price * Count;
        }
    }
}
