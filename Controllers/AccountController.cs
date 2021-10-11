using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using MyAspProject.Data;

namespace MyAspProject.Controllers
{
    public class AccountController : Controller
    {
        CookieOptions co = new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddDays(1)
        };

        private MyAspDbContext _context;

        public AccountController(MyAspDbContext context)
        {
            _context = context;
        }

        public IActionResult Main()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            foreach (var user in _context.Users)
            {
                if (username == user.UserName && password == user.Password)
                {
                    HttpContext.Response.Cookies.Append("FirstName", user.FirstName, co);
                    HttpContext.Response.Cookies.Append("LastName", user.LastName, co);
                    HttpContext.Response.Cookies.Append("userName", user.UserName, co);

                    return RedirectToAction("index", "Home", new { area = "" });
                }
            }

            TempData["errorLogIn"] = "חשבון לא נמצא";
            TempData["UserName"] = username;
            TempData["Password"] = password;

            return RedirectToAction("index", "Home", new { area = "" });
        }

        [HttpGet]
        public IActionResult Logout()
        { 
            foreach (var item in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(item.Key);
            }
            
            return RedirectToAction("index", "Home", new { area = "" });
        }
    }
}
