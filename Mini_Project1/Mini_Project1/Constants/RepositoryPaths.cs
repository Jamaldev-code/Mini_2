namespace Mini_Project1.Constants
{
    internal static class RepositoryPaths
    {
        private const string Folder = "Repository";

        public static readonly string Products = Path.Combine(Folder, "products.json");
        public static readonly string Orders = Path.Combine(Folder, "orders.json");
        public static readonly string Categories = Path.Combine(Folder, "categories.json"); // YENİ
    }
}
