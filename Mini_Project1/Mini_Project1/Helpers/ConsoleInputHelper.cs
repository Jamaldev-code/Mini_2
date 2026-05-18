
using Mini_Project1.Models;
using Mini_Project1.Services;

namespace Mini_Project1.Helpers
{
    internal static class ConsoleInputHelper
    {
        
        public static bool TryReadEmail(out string email)
        {
            Console.Write("Your Email: ");
            email = Console.ReadLine()?.Trim() ?? string.Empty;

            if (EmailValidator.IsValid(email)) return true;

            Console.WriteLine("[Error] Email must contain '@' character.");
            return false;
        }

        public static bool TryReadPhone(out string phoneNumber)
        {
            Console.Write("Enter Phone Number: ");
            phoneNumber = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                Console.WriteLine("[Error] Phone number cannot be empty.");
                return false;
            }
            if (!PhoneValidator.IsValid(phoneNumber))
            {
                Console.WriteLine("[Error] Invalid Azerbaijan phone number.(+994123456789)");
                return false;
            }
           
            return true;
        }

        public static bool TryReadProduct(
            ProductServices productService,
            out Product? product,
            out int count)
        {
            product = null;
            count = 0;

            Console.Write("\nProduct ID (0 to finish): ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid productId))
            {
                Console.WriteLine("[Error] Invalid ID.");
                return false;
            }

            if (productId == Guid.Empty) return false;

            product = productService.FindById(productId);
            if (product == null)
            {
                Console.WriteLine("[Error] Product not found.");
                return false;
            }

            if (product.Stock == 0)
            {
                Console.WriteLine($"[Error] '{product.Name}' is out of stock.");
                return false;
            }

            Console.Write($"How many '{product.Name}' (available: {product.Stock})? ");
            if (!int.TryParse(Console.ReadLine(), out count) || count <= 0)
            {
                Console.WriteLine("[Error] Count must be a positive number.");
                return false;
            }

            if (count > product.Stock)
            {
                Console.WriteLine($"[Error] Not enough stock. Available: {product.Stock}");
                return false;
            }

            return true;
        }

        public static bool TryReadOrderStatus(out OrderStatus status, out Guid id)
        {
            status = default;
            Console.Write("Order ID: ");

            if (!Guid.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("[Error] Invalid ID format.");
                return false;
            }

            var statuses = Enum.GetValues<OrderStatus>();
            Console.WriteLine("Select new status:");
            for (int i = 0; i < statuses.Length; i++)
                Console.WriteLine($"  {i + 1}. {statuses[i]}");

            Console.Write("Choice: ");
            if (!int.TryParse(Console.ReadLine(), out int choice) ||
                choice < 1 || choice > statuses.Length)
            {
                Console.WriteLine("[Error] Invalid choice.");
                return false;
            }

            status = statuses[choice - 1];
            return true;
        }
    }
}