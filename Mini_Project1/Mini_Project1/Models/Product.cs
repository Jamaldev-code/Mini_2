namespace Mini_Project1.Models
{
    internal class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }   // Effektiv qiymət

        // Endirim dəstəyi
        public decimal OriginalPrice { get; set; } = 0;   // 0 = endirim yoxdur
        public decimal DiscountPercent { get; set; } = 0;   // 0-100

        public int Stock { get; set; }

        public bool HasDiscount => DiscountPercent > 0 && OriginalPrice > 0;
    }
}
