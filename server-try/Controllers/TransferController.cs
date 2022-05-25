using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using server_try.Data;
using server.Models;
using Message = server.Models.Message;

namespace server_try.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : Controller
    {
        private readonly server_tryContext _context;

        public TransferController(server_tryContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Dictionary<string, string> data)
        {
            string from = data["from"];
            string to = data["to"];
            string content = data["content"];
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.Id== to);
            if (currentUser == null)
            {
                return NotFound();
            }
            var currentContact = _context.Contact.Include(x => x.ContactMessages).Where(u => u.id == from && u.UserId == currentUser.Id).FirstOrDefault();
            if (currentContact == null)
            {
                return NotFound();
            }
            var newMessage = new Message(content, false);
            currentContact.ContactMessages.Insert(0, newMessage);
            currentContact.last = content;
            DateTime date1 = DateTime.UtcNow;
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
            DateTime date2 = TimeZoneInfo.ConvertTime(date1, tz);
            currentContact.lastdate = date2.ToString("o");
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
