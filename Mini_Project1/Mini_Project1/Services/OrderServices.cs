using Mini_Project1.Helpers;
using Mini_Project1.Models;


namespace Mini_Project1.Services
{
    internal class OrderServices
    {
        private static readonly string path = "orders.json";

        public List<Order> Orders { get; set; }

        public OrderServices()
        {
            Orders = FileHelper.ReadFromFile<List<Order>>(path);

            if (Orders == null)
            {
                Orders = new List<Order>();
            }
        }

        public void Create(Order order)
        {
            Orders.Add(order);

            FileHelper.WriteToFile(path, Orders);
        }

        public void ShowAll()
        {
            foreach (var order in Orders)
            {
                order.PrintInfo();

                Console.WriteLine("--------------------");
            }
        }

        public void ChangeStatus(Guid id, OrderStatus status)
        {
            Order order = Orders.FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                Console.WriteLine("Order not found");
                return;
            }

            order.Status = status;

            FileHelper.WriteToFile(path, Orders);

            Console.WriteLine("Status changed");
        }
    }
}
