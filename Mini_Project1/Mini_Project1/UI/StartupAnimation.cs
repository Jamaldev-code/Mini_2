namespace Mini_Project1.UI
{
    internal static class StartupAnimation
    {
        public static void Show()
        {
            Console.Title = "MINI SHOP";

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Magenta;

            TypeLine("███╗   ███╗██╗███╗   ██╗██╗");
            TypeLine("████╗ ████║██║████╗  ██║██║");
            TypeLine("██╔████╔██║██║██╔██╗ ██║██║");
            TypeLine("██║╚██╔╝██║██║██║╚██╗██║██║");
            TypeLine("██║ ╚═╝ ██║██║██║ ╚████║██║");
            TypeLine("╚═╝     ╚═╝╚═╝╚═╝  ╚═══╝╚═╝");

            Console.ResetColor();

            Console.WriteLine();

            Loading("Initializing system");
            Loading("Loading products");
            Loading("Loading orders");
            Loading("Preparing dashboard");

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine();
            Console.WriteLine("✔ System Ready!");

            Console.ResetColor();

            Thread.Sleep(1000);
        }

        private static void TypeLine(string text)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(2);
            }

            Console.WriteLine();
        }

        private static void Loading(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write(text);

            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(300);
                Console.Write(".");
            }

            Console.WriteLine();

            Console.ResetColor();
        }
    }
}