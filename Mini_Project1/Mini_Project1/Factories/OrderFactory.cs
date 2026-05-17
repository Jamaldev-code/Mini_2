using Mini_Project1.Models;

namespace Mini_Project1.Factories
{
    internal static class OrderFactory
    {
        public static OrderItem CreateItem(Product product, int count)
            => new()
            {
                Id = Guid.NewGuid(),
                Product = product,
                Price = product.Price,
                ProductCount = count,
                SubTotal = product.Price * count
            };

        public static Order CreateOrder(string email, string phoneNumber, List<OrderItem> items)
            => new()
            {
                Id = Guid.NewGuid(),
                Email = email,
                PhoneNumber = phoneNumber,
                Items = items,
                Total = items.Sum(i => i.SubTotal),
                Status = OrderStatus.Pending,
                OrderedAt = DateTime.Now
            };
    }
}