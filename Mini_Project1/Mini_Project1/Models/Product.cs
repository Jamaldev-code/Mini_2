

namespace Mini_Project1.Models
{
    internal class Product
    {

        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public void PrintInfo()
        {
            // Stock 0 olduqda "Out of Stock", deyilsə say göstərilir
            string stockDisplay = Stock == 0 ? "Out of Stock" : Stock.ToString();

            Console.WriteLine($"  ID    : {Id}");
            Console.WriteLine($"  Name  : {Name}");
            Console.WriteLine($"  Price : ${Price:F2}");
            Console.WriteLine($"  Stock : {stockDisplay}");
        }
    }
}
