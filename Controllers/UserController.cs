using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAspProject.Data;
using MyAspProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;



namespace MyAspProject.Controllers
{
    
    public class UserController : Controller
    {
        CookieOptions co = new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddDays(1)
        };
        private MyAspDbContext _context;

        public UserController(MyAspDbContext context)
        {
            _context = context;
        }
       
        public IActionResult Registration()
        {

            if (HttpContext.Request.Cookies["firstName"] != null)
            {
                foreach (var user in _context.Users)
                {
                    if (user.FirstName == HttpContext.Request.Cookies["firstName"])
                    {
                        return View(user);
                    }
                }
            }
            return View(new User());
        }

        public IActionResult Create([Bind("Id,FirstName,LastName,Birthday,Email,UserName,Password,ConfirmPassword")] User user)
        {
            bool IsUserExists = _context.Users.Where(u => u.UserName == user.UserName).Any<User>();
            if (IsUserExists)
            {
                user.UserName = "שם המשתמש שייך ללקוח אחר, אנא נסה שוב";
                return View("Registration", user);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    HttpContext.Response.Cookies.Append("firstName", user.FirstName, co);
                    HttpContext.Response.Cookies.Append("lastName", user.LastName, co);
                    HttpContext.Response.Cookies.Append("userName", user.UserName, co);


                    _context.Users.Add(user);
                    _context.SaveChanges();

                    return View(user);
                }
                else
                {
                    TempData["ErrorsKeys"] = FindErrorsKeys();
                    return View("Registration", user);
                }
            }
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

        public IActionResult Update([Bind("Id,FirstName,LastName,Birthday,Email,UserName,Password,ConfirmPassword")] User user)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Response.Cookies.Append("firstName", user.FirstName, co);
                HttpContext.Response.Cookies.Append("lastName", user.LastName, co);
                HttpContext.Response.Cookies.Append("userName", user.UserName, co);

                User updUser = _context.Users.Where(p => p.Id == user.Id).FirstOrDefault();
                updUser.FirstName = user.FirstName;
                updUser.LastName = user.LastName;
                updUser.Birthday = user.Birthday;
                updUser.Password = user.Password;
                updUser.Email = user.Email;

                _context.SaveChanges();

                return View(updUser);
            }
            else
            {
                TempData["IsValid"] = "ModelIsNotValid";
                TempData["ErrorsKeys"] = FindErrorsKeys();
                return View("Registration", user);
            }
        }
    }
}
