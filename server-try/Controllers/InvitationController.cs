using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server_try.Data;

namespace server_try.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationController : Controller
    {
        private readonly server_tryContext _context;

        public InvitationController(server_tryContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Dictionary<string, string> data)
        {
            string from = data["from"];
            string to = data["to"];
            string server = data["server"];
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.Id == to);
            if (currentUser == null)
            {
                return NotFound();
            }
            var checkIfAlreadyContact = currentUser.ContactsList.Where(m => m.id == from);
            if (checkIfAlreadyContact.Any())
            {
                return BadRequest();
            }
            var addedContact = new Contact(from, currentUser.Id, from, server);
            currentUser.ContactsList.Add(addedContact);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
        // adds lastdate to new contact so the contact will appear at the top of contact list
        [HttpPost("AddContact")]
        public async Task<IActionResult> AddContact([FromBody] Dictionary<string, string> data)
        {
            string from = data["from"];
            string to = data["to"];
            string server = data["server"];
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.Id == to);
            if (currentUser == null)
            {
                return NotFound();
            }
            var checkIfAlreadyContact = currentUser.ContactsList.Where(m => m.id == from);
            if (checkIfAlreadyContact.Any())
            {
                return BadRequest();
            }
            var addedContact = new Contact(from, currentUser.Id, from, server);
            DateTime date1 = DateTime.UtcNow;
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
            DateTime date2 = TimeZoneInfo.ConvertTime(date1, tz);
            addedContact.lastdate = date2.ToString("o");
            currentUser.ContactsList.Add(addedContact);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}