using System.Linq;
using System.Net.Mail;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using GreenShop.Contexts;
using GreenShop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GreenShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopContext _context;
        private readonly SmtpClient _mail;
        private readonly IWebHostEnvironment _env;

        public ShopController(ShopContext context, SmtpClient mail, IWebHostEnvironment env)
        {
            _mail = mail;
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Where(product => product.IsDeleted==false).Select(product => new ProductViewModel
            {
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Caption = product.Caption,
                Stock = product.Stock,
                Price = product.Price,
                Id = product.Id
            }).ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(ProductViewModel form)
        {
            if (ModelState.IsValid)
            {
                var newProduct = new Product(form.Name, form.Caption, form.ImageUrl, form.Price, form.Stock);
                _context.Products.Add(newProduct);
                _context.SaveChanges();
                TempData["OperationStatus"] = "Success";
            }
            else
            {
                TempData["OperationStatus"] = "Fail";

            }

            return View(form);
        }


        public IActionResult RemoveProduct(int id)
        {
            var toBeRemoved = _context.Products.FirstOrDefault(x => x.Id == id);
            if (toBeRemoved != null)
            {
                TempData["OperationStatus"] = "Success";
                toBeRemoved.IsDeleted = true;
                _context.SaveChanges();
            }
            else
                TempData["OperationStatus"] = "Fail";

            return View();
        }

        public IActionResult ShowProduct(int id)
        {
            var item = _context.Products.FirstOrDefault(x => x.Id == id);
            if (item == null) return View("Error");
            var productViewModel = new ProductViewModel()
            {
                Name = item.Name, Caption = item.Caption, ImageUrl = item.ImageUrl,
                Stock = item.Stock, Price = item.Price, Id = item.Id
            };
            return View(productViewModel);


        }

        [HttpGet]
        public IActionResult EditProduct (int id)
        {
            var toBeEdited = _context.Products.FirstOrDefault(x => x.Id == id);
            
            if (toBeEdited != null)
            {
                return View(new ProductViewModel()
                {
                    Name = toBeEdited.Name,Caption = toBeEdited.Caption,Stock = toBeEdited.Stock,ImageUrl = toBeEdited.ImageUrl,
                    Price = toBeEdited.Price
                });
            }
            else
            {
                return View("Error");
            }
        }
        [HttpPost]
        public IActionResult EditProduct(int id,ProductViewModel form)
        {
            var toBeEdited = _context.Products.FirstOrDefault(x => x.Id == id);
            if (toBeEdited != null && ModelState.IsValid)
            {
                toBeEdited.Name = form.Name;
                toBeEdited.Caption = form.Caption;
                toBeEdited.ImageUrl = form.ImageUrl;
                toBeEdited.Price = form.Price;
                toBeEdited.Stock = form.Stock;
                TempData["OperationStatus"] = "Success";
                _context.Products.Update(toBeEdited);
                _context.SaveChanges();
                return View();
            }
            else
            {
                TempData["OperationStatus"] = "Fail";
                return View(form);
            }

        }

        [HttpPost]
        public async Task<RedirectResult> MailProduct(string address,long id)
        {
            var product = _context.Products.SingleOrDefault(x => x.Id == id);
            if (product != null)
            {
                var subject = $"Green Shop Product ({product.Name})";
                var body = $"<h1>{product.Name}</h1>\n<img src={product.ImageUrl}>\n{product.Caption}\nPrice: {product.Price}\nIn Stock: {product.Stock}\n\nLink: {_env.WebRootPath}/Products/{id}";
                await _mail.SendMailAsync(new MailMessage("Green Shop",
                address, subject, body));
                TempData["OperationStatus"] = "Success";
            }
            else TempData["OperationStatus"] = "Fail";
            return Redirect("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _mail.Dispose();
            base.Dispose(disposing);
        }
    }
}
