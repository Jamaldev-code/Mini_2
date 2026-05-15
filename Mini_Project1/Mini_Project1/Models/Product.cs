

namespace Mini_Project1.Models
{
    internal class Product
    {
        private static int _id;

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public Product(string name, decimal price, int stock)
        {
            _id++;

            Id = _id;
            Name = name;
            Price = price;
            Stock = stock;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Id: {Id}, Name: {Name}, Price: {Price}, Stock: {Stock}");

        }

    }
}
