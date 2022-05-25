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
                     select new { ID = data.id, name = data.name, server = data.server, last = data.last, lastdate = data.lastdate })
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
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                return NotFound();
            }
            var checkIfAlreadyContact = currentUser.ContactsList.Where(m => m.id == id);
            if (checkIfAlreadyContact.Any())
            {
                return BadRequest();
            }
            var addedContact = new Contact(id, currentUser.UserName, name, server);
            currentUser.ContactsList.Insert(0, addedContact);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }





        // adds lastdate to new contact so the contact will appear at the top of contact list
        [HttpPost("AddContact")]
        public async Task<IActionResult> AddContact([FromBody] Dictionary<string, string> data)
        {
            string id = data["id"];
            string name = data["name"];
            string server = data["server"];
            string user = data["user"];
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                return NotFound();
            }
            var checkIfAlreadyContact = currentUser.ContactsList.Where(m => m.id == id);
            if (checkIfAlreadyContact.Any())
            {
                return BadRequest();
            }
            var addedContact = new Contact(id, currentUser.UserName, name, server);
            DateTime date1 = DateTime.UtcNow;
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
            DateTime date2 = TimeZoneInfo.ConvertTime(date1, tz);
            addedContact.lastdate = date2.ToString("o");
            currentUser.ContactsList.Insert(0, addedContact);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        // GET: Contacts/:5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, string user)
        {
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                return NotFound();
            }
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = currentUser.ContactsList.Where(m => m.id == id);
            if (contact == null)
            {
                return NotFound();
            }
            var ret =
                from data in contact
                select new { ID = data.id, name = data.name, server = data.server, last = data.last, lastdate = data.lastdate };

            return Json(ret.ElementAt(0));
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Dictionary<string, string> data)
        {
            string name = data["name"];
            string server = data["server"];
            string user = data["user"];
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                return NotFound();
            }
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = currentUser.ContactsList.Where(m => m.id == id).FirstOrDefault();
            if (contact == null)
            {
                return NotFound();
            }
            contact.name = name;
            contact.server = server;
            _context.Contact.Update(contact);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromBody] string user, string id)
        {

            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                return NotFound(); ;
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
            return StatusCode(StatusCodes.Status204NoContent);
        }

    }
}
