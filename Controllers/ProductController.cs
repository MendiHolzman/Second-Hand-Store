using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MyAspProject.Data;
using MyAspProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace MyAspProject.Controllers
{
    public class ProductController : Controller
    {
        private MyAspDbContext _context;
        private readonly IWebHostEnvironment _iWebHost;

        public ProductController(MyAspDbContext context, IWebHostEnvironment iWebHost)
        {
            _context = context;
            _iWebHost = iWebHost;
        }

        public IActionResult AddToShoppingCart(int id)
        {
            UserProductViewModel up = new UserProductViewModel();
            up.Product = _context.Products.FirstOrDefault(p => p.Id == id);

            var cfn = HttpContext.Request.Cookies["userName"];
            if (cfn != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == cfn);
                up.Product.State = user.Id;
                _context.SaveChanges();
                TempData["addItemToShoppingCart"] = "success";

                return RedirectToAction("Index", "Home");
            }
            else
            {
                up.Product.State = -1;
                _context.SaveChanges();
                TempData["addItemToShoppingCart"] = "success";

                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult ShoppingCart()
        {
            List<Product> ShoppingCartList = new List<Product>();

            UserProductViewModel up = new UserProductViewModel();
            up.Product = new Product();
            up.User = new User();

            var cfn = HttpContext.Request.Cookies["userName"];
            if (cfn != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == cfn);

                foreach (var item in _context.Products)
                {
                    if (item.State == user.Id)
                    {
                        ShoppingCartList.Add(item);
                    }
                }

                return View(ShoppingCartList);
            }
            else
            {
                foreach (var item in _context.Products)
                {
                    if (item.State == -1)
                    {
                        ShoppingCartList.Add(item);
                    }
                }

                return View(ShoppingCartList);
            }
        }

        public IActionResult PostAnAd()
        {
            UserProductViewModel up = new UserProductViewModel();
            up.Product = new Product();
            return View(up);
        }

        public async Task<IActionResult> AdFormDataProcessingAsync([Bind("Id,Title,ShortDescription,LongDescription,Price,Image1,Image2,Image3")] Product product, IFormFile FImage1, IFormFile FImage2, IFormFile FImage3)
        {
            UserProductViewModel up = new UserProductViewModel();
            up.Product = new Product();
            if (ModelState.IsValid)
            {
                up.User = new User();

                foreach (var item in _context.Users)
                {
                    if (item.UserName == HttpContext.Request.Cookies["userName"])
                    {
                        up.User = item;
                    }
                }
                product.Date = DateTime.Today;
                product.OwnerId = up.User.Id;
                up.Product = product;

                if (FImage1 != null)
                {
                    await CopyImageFromDesktopToImagesFolder(FImage1);
                    up.Product.Image1 = FImage1.FileName;
                }
                if (FImage2 != null)
                {
                    await CopyImageFromDesktopToImagesFolder(FImage2);
                    up.Product.Image2 = FImage2.FileName;
                }
                if (FImage3 != null)
                {
                    await CopyImageFromDesktopToImagesFolder(FImage3);
                    up.Product.Image3 = FImage3.FileName;
                }

                _context.Products.Add(product);
                _context.SaveChanges();
                return View(up);
            }
            else
            {
                TempData["IsValid"] = "ModelIsNotValid";
                TempData["ErrorsKeys"] = FindErrorsKeys();

                up.Product = product;
                return View("PostAnAd", up);
            }
        }

        private async Task CopyImageFromDesktopToImagesFolder(IFormFile FImage)
        {
            var saveImg = Path.Combine(_iWebHost.WebRootPath, "Images", FImage.FileName);
            var stream = new FileStream(saveImg, FileMode.Create);
            await FImage.CopyToAsync(stream);
        }

        public List<string> FindErrorsKeys()
        {
            List<string> KeysErrors = new List<string>();
            foreach (var modelStateKey in ViewData.ModelState.Keys)
            {
                var modelStateVal = ViewData.ModelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {
                    if (error != null)
                    {
                        KeysErrors.Add(modelStateKey);
                    }
                }
            }
            return KeysErrors;
        }

        public IActionResult MoreDetails(int id)
        {
            UserProductViewModel up = new UserProductViewModel();
            up.Product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (id < 1 || id > _context.Products.Count())
            {
                TempData["errorMoreDetails"] = "Error: Id is not valid";
                return RedirectToAction("Index", "Home");
            }
            else if (up.Product != null)
            {
                up.User = _context.Users.FirstOrDefault(u => u.Id == up.Product.OwnerId);
                return View(up);
            }
            TempData["errorMoreDetails"] = "Error: Product is null";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Buying(string selfList)
        {
            string[] productsId = selfList.Split(" ");
            foreach (var Id in productsId)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == int.Parse(Id));
                product.State = -2;
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult RemoveFromBasket(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            product.State = 0;
            _context.SaveChanges();
            TempData["RemoveItem"] = "Yes";
            return RedirectToAction("ShoppingCart", "Product");
        }
    }
}
