using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using GreenShop.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace GreenShop.Controllers
{
    public class ShareController : Controller
    {
        private readonly ShopContext _context;
        private readonly SmtpClient _mail;
        [HttpPost]
        public async void ShareProductByEmail(long productId, string targetEmail)
        {
            var product = _context.Products.SingleOrDefault(x => x.Id == productId);
            if (product != null)
            {
                var subject = $"Green Shop Product ({product.Name})";

                var body =
                    $"<h1>{product.Name}</h1>\n<img src={product.ImageUrl}>\n{product.Caption}\nPrice: {product.Price}\nIn Stock: {product.Stock}\n\nLink: localhost:5001/Products/{HttpContext.Request.Query["id"]}";
                await _mail.SendMailAsync(new MailMessage("Green Shop",
                    targetEmail, subject, body));

                TempData["OperationStatus"] = "Success";
            }
            else
            {
                TempData["OperationStatus"] = "Fail";
            }

            Redirect(Url.Action("Index", "Shop"));
        }
    }
}
