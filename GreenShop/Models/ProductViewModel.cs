
using System.ComponentModel.DataAnnotations;

namespace GreenShop.Models
{
    public class ProductViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Caption is required")]
        public string Caption { get; set; }
        [Display(Name = "Image Url")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }


        [Required(ErrorMessage = "Unit price is required")]
        [DataType(DataType.Currency)]
        [Range(0.01, 100_000_000_000)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Stock count is required")]
        [Range(1,10_000_000)]
        public int Stock { get; set; }

    }
}
