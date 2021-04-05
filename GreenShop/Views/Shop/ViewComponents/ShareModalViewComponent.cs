using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using GreenShop.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GreenShop.Views.Shop.ViewComponents
{
    public class ShareModalViewComponent : ViewComponent
    {
        private readonly SmtpClient _mail;
        private readonly IWebHostEnvironment _env;
        private readonly ShopContext _context;

        public ShareModalViewComponent(ShopContext context, SmtpClient mail, IWebHostEnvironment env)
        {
            _env = env;
            _mail = mail;
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View("~/Views/Shop/ViewComponents/ShareModalViewComponent.cshtml");
        }
    }
}
