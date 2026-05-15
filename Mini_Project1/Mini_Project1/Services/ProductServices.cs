
using Mini_Project1.Helpers;
using Mini_Project1.Models;

namespace Mini_Project1.Services
{
    internal class ProductServices
    {

        private const string path = "products.json";

        public List<Product> Products { get; set; }

        public ProductServices()
        {
            Products = FileHelper.ReadFromFile<List<Product>>(path);

            if (Products == null)
            {
                Products = new List<Product>();
            }
        }

        public void Create(Product product)
            {
                bool result = Products.Any(x => x.Name.ToLower() == product.Name.ToLower());

                if (result)
                {
                    Console.WriteLine("Product already exists");
                    return;
                }

                Products.Add(product);

                FileHelper.WriteToFile(path, Products);

                Console.WriteLine("Product created");
            }

        public void Delete(int id)
        {
            Product product = Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                Console.WriteLine("Product not found");
                return;
            }

            Products.Remove(product);

            FileHelper.WriteToFile(path, Products);
        }

        public Product GetById(int id)
        {
            return Products.FirstOrDefault(x => x.Id == id);
        }

        public void ShowAll()
        {
            foreach (var product in Products)
            {
                product.PrintInfo();

                if (product.Stock == 0)
                {
                    Console.WriteLine("Out Of Stock");
                }

                Console.WriteLine("-------------------");
            }
        }

        public void Refill(int id, int count)
        {
            Product product = Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                Console.WriteLine("Product not found");
                return;
            }

            product.Stock += count;

            FileHelper.WriteToFile(path, Products);

            Console.WriteLine("Stock updated");
        }
        public List<Product> GetAll()
        {
            return Products;
        }
    }
    }
