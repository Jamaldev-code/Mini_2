
using Mini_Project1.Interfaces;
using Mini_Project1.Services;
using Mini_Project1.UI;



namespace Mini_Project1.Methods
{
    internal class Appmanagement
    {
        private readonly ProductServices _productService;
        private readonly OrderServices _orderService;


        public Appmanagement(IFileService fileService)
        {
            _productService = new ProductServices(fileService);
            _orderService = new OrderServices(fileService, _productService);
        }

        public void Run()
        {
            while (true)
            {
                MenuDisplay.Show();
                string choice = Console.ReadLine()?.Trim();
                bool continueLoop = HandleChoise(choice);
                if (!continueLoop) break;
                MenuDisplay.PressAnyKey();
            }
        }

        private bool HandleChoise(string choice)
        {
            switch (choice)
            {
                case "1": _productService.CreateProduct(); break;
                case "2": _productService.DeleteProduct(); break;
                case "3": _productService.GetProductById(); break;
                case "4": _productService.ShowAllProducts(); break;
                case "5": _productService.RefillProduct(); break;
                case "6": _orderService.OrderProduct(); break;
                case "7": _orderService.ShowAllOrders(); break;
                case "8": _orderService.ChangeOrderStatus(); break;
                case "0":
                    Console.WriteLine("See you later! Goodbye!");
                    return false;   // Döngüdən çıx siqnalı
                default:
                    Console.WriteLine("[Error] Invalid choice. Please try again.");
                    break;
            }

            return true;   // Döngünü davam et
        }
   
    }
}
