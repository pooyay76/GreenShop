
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
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Unit price is required")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Stock count is required")]
        public int Stock { get; set; }

    }
}
