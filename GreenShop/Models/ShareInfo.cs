using System.ComponentModel.DataAnnotations;

namespace GreenShop.Models
{
    public class ShareInfo
    {

        [DataType(DataType.EmailAddress)]
        public string TargetEmail { get; set; }
    }
}
