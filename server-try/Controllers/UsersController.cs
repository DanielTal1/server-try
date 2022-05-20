using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server_try.Data;

namespace server_try.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly server_tryContext _context;

        public UsersController(server_tryContext context)
        {
            _context = context;
        }

        // GET: Users
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(_context.User.Include(x => x.ContactsList));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Dictionary<string, string> data)
        {
            string Username = data["Username"];
            string Password = data["Password"];
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserName == Username && u.Password == Password);
            if (user != null)
            {
                return Json(user);
            }
            return Json("Failed");
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var q = from u in _context.User
                        where u.UserName == user.UserName
                        select u;
                if (q.Count() > 0)
                {
                    return Json("Error");
                }
                else
                {
                    _context.User.Add(user);
                    await _context.SaveChangesAsync();
                }
            }
            return Json("Success");
        }

    }
}
