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

        public string PhoneNumber { get; set; } = string.Empty;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime OrderedAt { get; set; }

       
    }
}
