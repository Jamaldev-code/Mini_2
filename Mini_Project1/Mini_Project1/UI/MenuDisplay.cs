

namespace Mini_Project1.UI
{
  
      public static class MenuDisplay
      {
            
            public static void Show()
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════╗");
                Console.WriteLine("║      MINI SHOP PROJECT       ║");
                Console.WriteLine("╠══════════════════════════════╣");
                Console.WriteLine("║  1. Create Product           ║");
                Console.WriteLine("║  2. Delete Product           ║");
                Console.WriteLine("║  3. Get Product By ID        ║");
                Console.WriteLine("║  4. Show All Products        ║");
                Console.WriteLine("║  5. Refill Product           ║");
                Console.WriteLine("║  6. Order Product            ║");
                Console.WriteLine("║  7. Show All Orders          ║");
                Console.WriteLine("║  8. Change Order Status      ║");
                Console.WriteLine("║  0. Exit                     ║");
                Console.WriteLine("╚══════════════════════════════╝");
                Console.Write("Choice: ");
            }

            public static void PressAnyKey()
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ResetColor();
                Console.ReadKey(intercept: true);
            }
      }
    
}
