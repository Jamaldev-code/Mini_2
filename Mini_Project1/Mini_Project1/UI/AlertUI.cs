using Mini_Project1.Models;

namespace Mini_Project1.UI
{
    internal static class AlertUI
    {
        public static void ShowLowStockNotification(Product product)
        {
            int left = Console.WindowWidth - 35;
            int top = 5;

            Console.ForegroundColor = ConsoleColor.Red;

            Console.SetCursorPosition(left, top);
            Console.WriteLine("╔══════════════════════════════╗");

            Console.SetCursorPosition(left, top + 1);
            Console.WriteLine("║      LOW STOCK ALERT        ║");

            Console.SetCursorPosition(left, top + 2);
            Console.WriteLine("╠══════════════════════════════╣");

            Console.SetCursorPosition(left, top + 3);
            Console.WriteLine(
                $"║ {product.Name,-10} : {product.Stock,-8}║");

            Console.SetCursorPosition(left, top + 4);
            Console.WriteLine("╚══════════════════════════════╝");

            Console.ResetColor();

            Console.Beep(500, 300);

            Thread.Sleep(2500);

            for (int i = 0; i < 5; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write(new string(' ', 35));
            }
        }
    }
}