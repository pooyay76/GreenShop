using System.Linq;
using GreenShop.Contexts;
using GreenShop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GreenShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopContext _context;
        private readonly IWebHostEnvironment _env;

        public ShopController(ShopContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Where(product => product.IsDeleted == false).Select(product =>
                new ProductViewModel
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
        public IActionResult EditProduct(int id)
        {
            var toBeEdited = _context.Products.FirstOrDefault(x => x.Id == id);

            if (toBeEdited != null)
            {
                return View(new ProductViewModel()
                {
                    Name = toBeEdited.Name, Caption = toBeEdited.Caption, Stock = toBeEdited.Stock,
                    ImageUrl = toBeEdited.ImageUrl,
                    Price = toBeEdited.Price
                });
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult EditProduct(int id, ProductViewModel form)
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



    }
}
