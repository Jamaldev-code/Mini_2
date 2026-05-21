namespace Mini_Project1.UI
{
    internal static class MenuDisplay
    {
        public static void Show()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║         MINI SHOP PROJECT              ║");
            Console.WriteLine("╠════════════════════════════════════════╣");
            Console.WriteLine("║  ── Mehsullar ──                       ║");
            Console.WriteLine("║   1. Create Product                    ║");
            Console.WriteLine("║   2. Delete Product                    ║");
            Console.WriteLine("║   3. Get Product By ID                 ║");
            Console.WriteLine("║   4. Show All Products                 ║");
            Console.WriteLine("║   5. Refill Product                    ║");
            Console.WriteLine("║   6. Update Product                    ║");
            Console.WriteLine("╠════════════════════════════════════════╣");
            Console.WriteLine("║  ── Sifarishler ──                     ║");
            Console.WriteLine("║   7. Order Product                     ║");
            Console.WriteLine("║   8. Show All Orders                   ║");
            Console.WriteLine("║   9. Change Order Status               ║");
            Console.WriteLine("║  10. Search Orders by Email            ║");
            Console.WriteLine("║  11. Cancel Order                      ║");
            Console.WriteLine("╠════════════════════════════════════════╣");
            Console.WriteLine("║  ── Ayarlar ve Hesabatlar ──           ║");
            Console.WriteLine("║  12. Category Management               ║");
            Console.WriteLine("║  13. Product Statistics                ║");
            Console.WriteLine("╠════════════════════════════════════════╣");
            Console.WriteLine("║   0. Cixish                            ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.Write("Secim: ");
        }

        public static void PressAnyKey()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nMenyuya qayitmaq ucun bir dume basin...");
            Console.ResetColor();
            Console.ReadKey(intercept: true);
        }
    }
}
