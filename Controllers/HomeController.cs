using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyAspProject.Data;
using MyAspProject.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyAspProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyAspDbContext _context;

        public HomeController(ILogger<HomeController> logger, MyAspDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder)
        {

            ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            var products = from p in _context.Products
                           where p.State == 0
                           select p;

            switch (sortOrder)
            {
                case "name":
                    products = products.OrderBy(p => p.Title);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.Title);
                    break;
                case "Date":
                    products = products.OrderBy(p => p.Date);
                    break;
                case "date_desc":
                    products = products.OrderByDescending(p => p.Date);
                    break;
                default:
                    products = products.OrderBy(p => p.Id);
                    break;
            }

            return View(await products.AsNoTracking().ToListAsync());
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
