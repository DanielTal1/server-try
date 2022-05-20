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
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.Id == to);
            if (currentUser == null)
            {
                return Json("couldnt find user");
            }
            var currentContact = _context.Contact.Include(x => x.ContactMessages).Where(u => u.id == from && u.UserId == currentUser.Id).FirstOrDefault();
            if (currentUser == null)
            {
                return Json("couldnt find contact");
            }
            var newMessage = new Message(content, false);
            currentContact.ContactMessages.Insert(0, newMessage);
            currentContact.last = content;
            currentContact.lastdate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
            await _context.SaveChangesAsync();
            return Json(currentContact.ContactMessages);
        }
    }
}
