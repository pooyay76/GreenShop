using System.Linq;
using GreenShop.Contexts;
using GreenShop.Models;
using GreenShop.Modules;
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

                var emailBuilder = new EmailBuilder(product, info.TargetEmail);
                try
                {
                    emailBuilder.SendEmail();
                    TempData["OperationStatus"] = "Success";
                }
                catch
                {
                    TempData["OperationStatus"] = "Fail";
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
