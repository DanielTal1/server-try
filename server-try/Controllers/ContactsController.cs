using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using server.Models;
using server_try.Data;

namespace server_try.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly server_tryContext _context;

        public ContactsController(server_tryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string user)
        {
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                //user wasnt found
                return NotFound();
            }
            var ret =
                    from value in
                    (from data in currentUser.ContactsList
                     orderby data.lastdate descending
                     select new { ID = data.id, name = data.name,server= data.server,last= data.last,lastdate=data.lastdate })
                                group value by value.name into g
                                select g.First();

            return Json(ret);
        }

        // POST: Contacts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Dictionary<string, string> data)
        {
            string id = data["id"];
            string name = data["name"];
            string server = data["server"];
            string user = data["user"];
            var currentUser = await _context.User.Include(x=>x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                return Json("user wasnt found");
            }
            var checkIfAlreadyContact= currentUser.ContactsList.Where(m => m.id == id);
            if (checkIfAlreadyContact.Any())
            {
                return Json("Already Contact");
            }
            var addedContact = new Contact(id, currentUser.Id, name, server);
            currentUser.ContactsList.Add(addedContact);
            await _context.SaveChangesAsync();
            return Json("Success");
        }

        // GET: Contacts/:5
        [HttpGet(":{id}")]
        public async Task<IActionResult> Get(string id, string user)
        {
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                return Json("user wasnt found");
            }
            if (id == null || _context.Contact == null)
            {
                return Json("contact wasnt found");
            }

            var contact = currentUser.ContactsList.Where(m => m.id == id);
            if (contact == null)
            {
                return Json("contact wasnt found");
            }
            var ret =
                from data in contact
                select new { ID = data.id, name = data.name, server = data.server, last = data.last, lastdate = data.lastdate };

            return Json(ret.ElementAt(0));
        }



        [HttpPut(":{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Dictionary<string, string> data)
        {
            string name = data["name"];
            string server = data["server"];
            string user = data["user"];
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                return Json("user wasnt found");
            }
            if (id == null || _context.Contact == null)
            {
                return Json("contact wasnt found");
            }

            var contact = currentUser.ContactsList.Where(m => m.id == id).FirstOrDefault();
            if (contact == null)
            {
                return Json("contact wasnt found");
            }
            contact.name = name;
            contact.server = server;
            _context.Contact.Update(contact);
            await _context.SaveChangesAsync();
            return Json("success");
        }


        [HttpDelete(":{id}")]
        public async Task<IActionResult> Delete([FromBody] string user, string id)
        {

            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                return BadRequest();
            }
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.Include(x => x.ContactMessages).FirstOrDefaultAsync(u => u.id == id && u.UserId == currentUser.Id);
            if (contact == null)
            {
                return NotFound();
            }
            _context.Contact.Remove(contact);
            await _context.SaveChangesAsync();
            return Json("success");
        }

    }
}
