namespace GreenShop.Models
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public bool IsDeleted { get; set; }

        public Product(string name, string caption, string imageUrl, double price, int stock, bool isDeleted=false)
        {
            Name = name;
            Caption = caption;
            ImageUrl = imageUrl;
            Price = price;
            Stock = stock;
            IsDeleted = isDeleted;
        }
    }
}
