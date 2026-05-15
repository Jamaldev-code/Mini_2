using Mini_Project1.Models;
using Mini_Project1.Services;


namespace Mini_Project1.Methods
{
    internal class Appmanagement
    {
        private ProductServices productService;


        public Appmanagement(ProductServices productService)
        {
            this.productService = productService;
        }

        public void ShowAllProducts()
        {

            List<Product> products = productService.GetAll();

            foreach (Product product in products)
            {
                product.PrintInfo();
            }

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            if (!email.Contains('@'))
            {
                Console.WriteLine("Invalid Email");
                return;
            }
            else if (!email.EndsWith(".com") && !email.EndsWith(".ru"))
            {
                Console.WriteLine("Invalid Email");
                return;
            }
        }
    }
}
