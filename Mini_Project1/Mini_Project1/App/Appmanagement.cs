using Mini_Project1.Abstractions;
using Mini_Project1.Services;
using Mini_Project1.UI;

namespace Mini_Project1.App
{
    internal class Appmanagement
    {
        private readonly CategoryService _categoryService;
        private readonly ProductServices _productService;
        private readonly OrderServices _orderService;

        public Appmanagement(IFileService fileService)
        {
            _categoryService = new CategoryService(fileService);
            _productService = new ProductServices(fileService, _categoryService);
            _orderService = new OrderServices(fileService, _productService);
        }

        public void Run()
        {
            while (true)
            {
                MenuDisplay.Show();
                string choice = Console.ReadLine()?.Trim() ?? string.Empty;
                bool cont = HandleChoice(choice);
                if (!cont) break;
                MenuDisplay.PressAnyKey();
            }
        }

        private bool HandleChoice(string choice)
        {
            switch (choice)
            {
                case "1": _productService.CreateProduct(); break;
                case "2": _productService.DeleteProduct(); break;
                case "3": _productService.GetProductById(); break;
                case "4": _productService.ShowAllProducts(); break;
                case "5": _productService.RefillProduct(); break;
                case "6": _productService.UpdateProduct(); break;  // endirim daxil

                case "7": _orderService.OrderProduct(); break;
                case "8": _orderService.ShowAllOrders(); break;
                case "9": _orderService.ChangeOrderStatus(); break;
                case "10": _orderService.SearchOrdersByEmail(); break;
                case "11": _orderService.CancelOrder(); break;

                case "12": _categoryService.ManageCategories(); break;
                case "13": _orderService.ShowProductStats(); break;

                case "0":
                    Console.WriteLine("Gorushenedek! Xudahafiz!");
                    return false;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Xeta] Yanlish secim.");
                    Console.ResetColor();
                    break;
            }
            return true;
        }
    }
}
