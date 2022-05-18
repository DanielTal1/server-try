using Microsoft.AspNetCore.Mvc;
using server_try.Data;
using server_try.Models;
using System.Diagnostics;

namespace server_try.Controllers
{
    public class HomeController : Controller
    {
        private readonly server_tryContext _context;

        public HomeController(server_tryContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return Json(_context.User);
        }

        public IActionResult Privacy()
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