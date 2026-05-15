using Mini_Project1.Methods;
using Mini_Project1.Models;
using Mini_Project1.Services;

namespace Mini_Project1
{
    internal class Program
    {

        private static readonly string email;

        static void Main(string[] args)
        {
            ProductServices productServices = new ProductServices();
            OrderServices orderServices = new OrderServices();

            while (true)
            {
                Console.WriteLine("1.Create Product\n2.Delete Product\n3.Get Product By Id\n4.Show All Product\n5.Refill Product\n6.Order Product\n7.Show All Orders\n8.Change Order Status\n\n0.Exit");


                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        productServices.Create();
                        //Console.WriteLine("Create Product");
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("Delete Product");
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("Get Product By Id");
                        break;

                    case "4":
                        Console.Clear();
                        productServices.ShowAll();
                        //Console.WriteLine("Show All Products");
                        break;
                    case "5":
                        Console.Clear();
                        Console.WriteLine("Refill Product");
                        break;
                    case "6":
                        Console.Clear();
                        Console.WriteLine("Order Product");
                        break;
                    case "7":
                        Console.Clear();
                        Console.WriteLine("Show All Orders");
                        break;
                    case "8":
                        Console.Clear();
                        Console.WriteLine("Change Order Status");
                        break;
                    case "0":
                        Console.Clear();
                        Console.WriteLine("Exiting the application... Goodbye!");
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid option. Please try again.");
                        break;





                        //List<OrderItem> items = new List<OrderItem>();

                        //while (true)
                        //{
                        //    Console.Write("Enter Product Id: ");

                        //    bool checkId = int.TryParse(Console.ReadLine(), out int productId);

                        //    if (!checkId)
                        //    {
                        //        Console.WriteLine("Invalid Id");
                        //        continue;
                        //    }

                        //    Product product = productService.GetById(productId);

                        //    if (product == null)
                        //    {
                        //        Console.WriteLine("Product not found");
                        //        continue;
                        //    }

                        //    Console.Write("Enter Count: ");

                        //    bool checkCount = int.TryParse(Console.ReadLine(), out int count);

                        //    if (!checkCount || count <= 0)
                        //    {
                        //        Console.WriteLine("Invalid Count");
                        //        continue;
                        //    }

                        //    if (product.Stock < count)
                        //    {
                        //        Console.WriteLine("Not enough stock");
                        //        continue;
                        //    }

                        //    product.Stock -= count;

                        //    OrderItem orderItem = new OrderItem(product, count);

                        //    items.Add(orderItem);

                        //    Console.Write("Add another product? (yes/no): ");

                        //    string answer = Console.ReadLine().ToLower();

                        //    if (answer != "yes")
                        //    {
                        //        break;
                        //    }

                        //}

                        //Order order = new Order(email, items);

                        //orderService.Create(order);

                        //FileHelper.WriteToFile("products.json", productService.Products);

                        //Console.WriteLine("Order created successfully");

                        //break;
                }
            }
        }
    }
}


