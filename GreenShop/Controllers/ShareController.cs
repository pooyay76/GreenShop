using System.Linq;
using System.Net;
using System.Net.Mail;
using GreenShop.Contexts;
using GreenShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GreenShop.Controllers
{
    public class ShareController : Controller
    {

        private readonly ShopContext _context;


        public ShareController(ShopContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult ShareProductByEmail(long id,ShareInfo info)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.Development.Json").Build();
            var product = _context.Products.SingleOrDefault(x => x.Id == id);
            if (product != null)
            {

                SmtpClient smtpClient = new SmtpClient(config["Email:Host"])
                {
                    Port = int.Parse(config["Email:Port"]),
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(config["Email:Username"],config["Email:Password"]),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(config["Email:Username"]),
                    Subject = $"Green Shop Product ({product.Name})",
                    Body = $"<h1>{product.Name}</h1>\n<img src={product.ImageUrl}>\n{product.Caption}\nPrice: {product.Price}\nIn Stock: {product.Stock}\n\n<a href={Url.Action("ShowProduct","Shop",new {id=product.Id})}></a>"
                };
                try
                {
                    smtpClient.Send(mailMessage);
                    TempData["OperationStatus"] = "Success";
                }
                catch
                {
                    TempData["OperationStatus"] = "Fail";
                }
                finally
                {
                    smtpClient.Dispose();
                }
            }
            else
            {
                TempData["OperationStatus"] = "Fail";
            }

            return RedirectToAction("Index", "Shop");
        }
    }
}
