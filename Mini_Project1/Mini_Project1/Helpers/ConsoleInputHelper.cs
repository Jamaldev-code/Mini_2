using Mini_Project1.Models;
using Mini_Project1.Services;
using System.Globalization;

namespace Mini_Project1.Helpers
{
    internal static class ConsoleInputHelper
    {
        private const int MaxFails = 2;

        /// <summary>
        /// Hər hansı giriş funksiyası uğursuz olduqda retry edir.
        /// MaxFails-dan sonra ana menyuya qayitmag teklif edilir.
        /// true = devam et, false = ana menuya qayit.
        /// </summary>
        public static bool ReadWithRetry(Func<bool> attempt)
        {
            int fails = 0;
            while (true)
            {
                if (attempt()) return true;
                if (++fails >= MaxFails)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\nAna menuya qayitmaq isteyirsiniz? (y/n): ");
                    Console.ResetColor();
                    if (Console.ReadLine()?.Trim().ToLower() == "y") return false;
                    fails = 0;
                }
            }
        }

        public static bool TryReadEmail(out string email)
        {
            Console.Write("Email: ");
            email = Console.ReadLine()?.Trim() ?? string.Empty;
            if (EmailValidator.IsValid(email)) return true;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Xeta] Email duzgun formatda deyil (nemuine: user@example.com).");
            Console.ResetColor();
            return false;
        }

        public static bool TryReadPhone(out string phoneNumber)
        {
            Console.Write("Telefon nomresi: ");
            phoneNumber = Console.ReadLine()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Xeta] Telefon nomresi bos ola bilmez.");
                Console.ResetColor();
                return false;
            }
            if (!PhoneValidator.IsValid(phoneNumber))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Xeta] Yanlish Azerbaycan telefon nomresi. Nemuine: +994501234567");
                Console.ResetColor();
                return false;
            }
            return true;
        }

        public static bool TryReadProduct(ProductServices productService, out Product? product, out int count)
        {
            product = null;
            count = 0;
            Console.Write("\nMehsul ID (bitirmek ucun 0 daxil edin): ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid productId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Xeta] Yanlish ID formati.");
                Console.ResetColor();
                return false;
            }
            if (productId == Guid.Empty) return false;

            product = productService.FindById(productId);
            if (product == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Xeta] Mehsul tapilmadi.");
                Console.ResetColor();
                return false;
            }
            if (product.Stock == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Xeta] '{product.Name}' stokda yoxdur.");
                Console.ResetColor();
                return false;
            }
            Console.Write($"Nece edet '{product.Name}' (movcud: {product.Stock})? ");
            if (!int.TryParse(Console.ReadLine(), out count) || count <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Xeta] Miqdarin musbat reqem olmalidir.");
                Console.ResetColor();
                return false;
            }
            if (count > product.Stock)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Xeta] Kifayet qeder stok yoxdur. Movcud: {product.Stock}");
                Console.ResetColor();
                return false;
            }
            return true;
        }

        public static bool TryReadOrderStatus(out OrderStatus status, out Guid id)
        {
            status = default;
            Console.Write("Sifaris ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Xeta] Yanlish ID formati.");
                Console.ResetColor();
                return false;
            }
            var statuses = Enum.GetValues<OrderStatus>();
            Console.WriteLine("Yeni status secin:");
            for (int i = 0; i < statuses.Length; i++)
                Console.WriteLine($"  {i + 1}. {statuses[i]}");
            Console.Write("Secim: ");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > statuses.Length)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Xeta] Yanlish secim.");
                Console.ResetColor();
                return false;
            }
            status = statuses[choice - 1];
            return true;
        }

        // FIX: Hem "." hem "," qebul edilen decimal parser
        public static bool TryReadDecimal(string prompt, out decimal value, decimal min = 0)
        {
            Console.Write(prompt);
            string raw = Console.ReadLine()?.Trim().Replace(',', '.') ?? "";
            if (!decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out value) || value < min)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Xeta] {min}-den boyuk reqem daxil edin (nemuine: 9.99).");
                Console.ResetColor();
                return false;
            }
            return true;
        }
    }
}
