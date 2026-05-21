using Mini_Project1.Models;

namespace Mini_Project1.Display
{
    internal static class ConsoleRenderer
    {
        private const int INNER = 76;
        private const int W_No = 2;
        private const int W_Name = 28;
        private const int W_Qty = 5;
        private const int W_Unit = 12;
        private const int W_Sub = 13;

        private static string Line(char l, char f, char r)
            => l + new string(f, INNER + 2) + r;

        private static string Trunc(string s, int max)
            => s.Length <= max ? s : s[..(max - 2)] + "\u2026";

        private static string StatusLabel(OrderStatus s) => s switch
        {
            OrderStatus.Pending => "Gozlemede",
            OrderStatus.Confirmed => "Tesdiklendi",
            OrderStatus.Completed => "Tamamlandi",
            OrderStatus.Cancelled => "Legv edildi",
            _ => s.ToString()
        };

        private static ConsoleColor StatusColor(OrderStatus s) => s switch
        {
            OrderStatus.Pending => ConsoleColor.Yellow,
            OrderStatus.Confirmed => ConsoleColor.Cyan,
            OrderStatus.Completed => ConsoleColor.Green,
            OrderStatus.Cancelled => ConsoleColor.DarkGray,
            _ => ConsoleColor.White
        };

        // ── PrintProduct ──────────────────────────────────────────────────────
        public static void PrintProduct(Product p)
        {
            string stockDisplay = p.Stock == 0 ? "Stokda yoxdur" : p.Stock.ToString();
            Console.WriteLine($"  ID         : {p.Id}");
            Console.WriteLine($"  Ad         : {p.Name}");
            Console.WriteLine($"  Kateqoriya : {p.Category}");
            if (p.HasDiscount)
            {
                Console.Write($"  Qiymet     : ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"${p.OriginalPrice:F2}  ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"${p.Price:F2}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  (-{p.DiscountPercent:F0}% endirim)");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"  Qiymet     : ${p.Price:F2}");
            }
            Console.WriteLine($"  Stok       : {stockDisplay}");
        }

        public static void PrintProductInline(Product p)
        {
            string stockDisplay = p.Stock == 0 ? "Tukenib" : p.Stock.ToString();
            string priceDisplay = p.HasDiscount
                ? $"${p.Price:F2} (-{p.DiscountPercent:F0}%)"
                : $"${p.Price:F2}";
            Console.WriteLine($"Ad: {p.Name,-18} | Qiymet: {priceDisplay,-18} | Stok: {stockDisplay}");
            Console.WriteLine($"  |     ID: {p.Id}");
        }

        // ── PrintOrder — qapalı box-drawing dizayn ────────────────────────────
        public static void PrintOrder(Order o, int index = 0)
        {
            string top = Line('*', '=', '*');
            string mid = Line('+', '=', '+');
            string rowSep = Line('+', '-', '+');
            string bot = Line('*', '=', '*');

            // Ustde Unicode box-drawing istifade edirik
            string topU = "\u2554" + new string('\u2550', INNER + 2) + "\u2557";
            string midU = "\u2560" + new string('\u2550', INNER + 2) + "\u2563";
            string rowSepU = "\u255f" + new string('\u2500', INNER + 2) + "\u2562";
            string botU = "\u255a" + new string('\u2550', INNER + 2) + "\u255d";
            string colSepU = BuildColSep();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(topU);

            // Bashliq: sifaris ID + tarix
            string idxStr = index > 0 ? $"#{index}  " : "";
            string idStr = $"Sifaris {idxStr}{o.Id.ToString()[..8]}...";
            string dateStr = o.OrderedAt.ToString("dd.MM.yyyy  HH:mm");
            int gap = INNER - idStr.Length - dateStr.Length;
            if (gap < 1) gap = 1;
            Console.WriteLine($"\u2551 {idStr}{new string(' ', gap)}{dateStr} \u2551");
            Console.WriteLine(midU);

            // Email + Status
            string emailTrunc = Trunc(o.Email, 26);
            string statusLbl = StatusLabel(o.Status);
            string left = $"  Email   : {emailTrunc,-28}";
            string right = $"  Status  : {statusLbl}";
            int pad = INNER - left.Length - right.Length;
            if (pad < 0) pad = 0;
            Console.Write($"\u2551{left}{right}{new string(' ', pad)}\u2551");
            Console.WriteLine();

            // Telefon
            string phoneContent = $"  Telefon : {o.PhoneNumber}";
            Console.WriteLine($"\u2551{phoneContent.PadRight(INNER)}\u2551");
            Console.WriteLine(midU);

            // Cedvel bashligi
            Console.ForegroundColor = ConsoleColor.White;
            string hdr = $"  {" #",W_No}  {"\u2502"}  {"Mehsul",-W_Name}  {"\u2502"}  {"Say",W_Qty}  {"\u2502"}  {"Vahid",W_Unit}  {"\u2502"}  {"Cemi",W_Sub}  ";
            Console.WriteLine($"\u2551{hdr.PadRight(INNER)}\u2551");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(colSepU);

            // Mehsul setirleri
            for (int i = 0; i < o.Items.Count; i++)
            {
                var item = o.Items[i];
                string nm = Trunc(item.Product.Name, W_Name);
                string qty = $"x{item.ProductCount}";
                string un = $"${item.Price:F2}";
                string sb = $"${item.SubTotal:F2}";

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($"\u2551  {i + 1,W_No}  \u2502  ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{nm,-W_Name}");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("  \u2502  ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{qty,W_Qty}");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("  \u2502  ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{un,W_Unit}");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("  \u2502  ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{sb,W_Sub}");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("  \u2551");

                if (i < o.Items.Count - 1)
                    Console.WriteLine(rowSepU);
            }

            // Umumi cem
            Console.WriteLine(midU);
            string totalLabel = "Umumi :";
            string totalVal = $"  ${o.Total:F2}  ";
            int tpad = INNER - 2 - totalLabel.Length - totalVal.Length;
            if (tpad < 1) tpad = 1;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"\u2551{new string(' ', tpad)}{totalLabel}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(totalVal);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\u2551");
            Console.WriteLine(botU);
            Console.ResetColor();
        }

        private static string BuildColSep()
        {
            // +--+---+--+---+--+---+--+---+--+
            int[] segs = { W_No + 2, W_Name + 4, W_Qty + 2, W_Unit + 4, W_Sub + 4 };
            var sb = new System.Text.StringBuilder();
            sb.Append('\u255e');   // ╞
            foreach (var seg in segs)
            {
                sb.Append(new string('\u2550', seg));   // ═
                if (sb.Length < INNER + 1)
                    sb.Append('\u256a');   // ╪
            }
            // Trim to fit and close
            string built = sb.ToString();
            if (built.Length > INNER + 1)
                built = built[..(INNER + 1)];
            return built.PadRight(INNER + 1, '\u2550') + '\u2561';   // ╡
        }
    }
}
