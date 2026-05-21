using Mini_Project1.Models;

namespace Mini_Project1.Helpers
{
    /// <summary>
    /// ID tələb edən bütün əməliyyatlarda ekranın yuxarısında
    /// məhsul siyahısını tam UUID-lərlə göstərir.
    /// </summary>
    internal static class ProductPanelHelper
    {
        // ── Sütun genişlikləri (daxili məzmun) ─────────────────────────────
        private const int W_Num = 3;
        private const int W_Name = 16;
        private const int W_Category = 10;
        private const int W_Price = 9;   // "$9999.00" formatı üçün
        private const int W_Stock = 5;
        private const int W_Id = 36;   // tam GUID

        // Ümumi sətir uzunluğu:
        // 2 + 3 + 3 + 16 + 3 + 10 + 3 + 9 + 3 + 5 + 3 + 36 + 2 = 98 simvol
        private const int TotalWidth = 98;

        // ── Ayırıcı sətirləri ────────────────────────────────────────────
        private static readonly string _top = BuildBorder('╔', '╦', '╗', '═');
        private static readonly string _mid = BuildBorder('╠', '╬', '╣', '═');
        private static readonly string _bot = BuildBorder('╚', '╩', '╝', '═');
        private static readonly string _hdr = BuildRow(" # ", "Name", "Category", "Price", "Stock", "ID");

        // ── Ana metod ────────────────────────────────────────────────────
        /// <summary>
        /// Ekranı təmizləyib məhsul panelini çəkir, sonra kursoru
        /// əməliyyat promtları üçün hazır vəziyyətdə buraxır.
        /// </summary>
        public static void Draw(IEnumerable<Product> products, string operationTitle)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(_top);
            Console.WriteLine(_hdr);
            Console.WriteLine(_mid);

            var list = products.ToList();
            if (list.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                string empty = $"║ {"No products found.",-W_Id + W_Price + W_Stock + W_Name + W_Category + W_Num + 15} ║";
                Console.WriteLine(empty);
            }

            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                PrintRow(i + 1, p);
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(_bot);
            Console.ResetColor();

            // Əməliyyat başlığı panelin altında
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n──── {operationTitle} ────");
            Console.ResetColor();
        }

        // ── Sətir çəkmə ──────────────────────────────────────────────────
        private static void PrintRow(int index, Product p)
        {
            string name = Trunc(p.Name, W_Name);
            string category = Trunc(p.Category, W_Category);
            string price = "$" + p.Price.ToString("F2");

            // Sol tərəf (rəngdən əvvəl)
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"║ {index,W_Num} │ ");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{name,-W_Name}");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(" │ ");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{category,-W_Category}");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(" │ ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{price,W_Price}");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(" │ ");

            // Stoka görə rəng
            if (p.Stock == 0)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (p.Stock <= 5)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.Green;

            Console.Write($"{p.Stock,W_Stock}");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(" │ ");

            // UUID — kopyalanabilər, tam görünür
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(p.Id.ToString());

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(" ║");
        }

        // ── Border builder ────────────────────────────────────────────────
        private static string BuildBorder(
            char left, char cross, char right, char fill)
        {
            string Seg(int w) => new string(fill, w + 2);

            return left
                + Seg(W_Num) + cross
                + Seg(W_Name) + cross
                + Seg(W_Category) + cross
                + Seg(W_Price) + cross
                + Seg(W_Stock) + cross
                + Seg(W_Id)
                + right;
        }

        private static string BuildRow(
            string num, string name, string cat, string price, string stock, string id)
        {
            return $"║ {num,W_Num} │ {name,-W_Name} │ {cat,-W_Category} │ {price,W_Price} │ {stock,W_Stock} │ {id,-W_Id} ║";
        }

        // ── Köməkçi ───────────────────────────────────────────────────────
        private static string Trunc(string s, int max)
            => s.Length <= max ? s : s[..(max - 1)] + "…";
    }
}
