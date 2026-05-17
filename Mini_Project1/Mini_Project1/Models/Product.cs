

namespace Mini_Project1.Models
{
    internal class Product
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }

       
    }
}
