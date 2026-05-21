using Mini_Project1.Models;

namespace Mini_Project1.UI
{
    internal static class AlertUI
    {
        /// <summary>
        /// FIX #4 – Cursor pozisiyası saxlanılır və alert silinəndən
        ///          sonra bərpa edilir. Console.Write istifadə edilir
        ///          (WriteLine deyil) — əks halda kursor sola keçərdi
        ///          və növbəti yazılar üst-üstə düşürdü.
        /// </summary>
        public static void ShowLowStockNotification(Product product)
        {
            // Cursor-u yadda saxla
            int savedLeft = Console.CursorLeft;
            int savedTop = Console.CursorTop;

            int left = Console.WindowWidth - 36;
            int top = 2;

            if (left < 0) left = 0; // dar terminal üçün

            Console.ForegroundColor = ConsoleColor.Red;

            WriteAt(left, top, "╔══════════════════════════════════╗");
            WriteAt(left, top + 1, "║        ⚠  LOW STOCK ALERT        ║");
            WriteAt(left, top + 2, "╠══════════════════════════════════╣");

            string namePart = product.Name.Length > 14 ? product.Name[..13] + "…" : product.Name;
            string stockPart = product.Stock.ToString();
            string line = $"║  {namePart,-14} : {stockPart,-6} qalıb       ║";
            WriteAt(left, top + 3, line);

            WriteAt(left, top + 4, "╚══════════════════════════════════╝");

            Console.ResetColor();

            try { Console.Beep(700, 250); } catch { /* Bəzi terminallarda Beep yoxdur */ }

            Thread.Sleep(2500);

            // Alert-i sil
            string blank = new string(' ', 37);
            for (int i = 0; i < 5; i++)
                WriteAt(left, top + i, blank);

            // Cursor-u bərpa et — bu olmadan növbəti yazılar qarışırdı
            Console.SetCursorPosition(savedLeft, savedTop);
        }

        private static void WriteAt(int left, int top, string text)
        {
            try
            {
                Console.SetCursorPosition(left, top);
                Console.Write(text);   // Write, WriteLine deyil!
            }
            catch (ArgumentOutOfRangeException) { /* terminal çox kiçikdirsə keç */ }
        }
    }
}
