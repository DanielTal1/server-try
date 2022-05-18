using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using server_try.Data;
using server.Models;
using Message = server.Models.Message;

namespace server_try.Controllers
{
    public class TransferController : Controller
    {
        private readonly server_tryContext _context;

        public TransferController(server_tryContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Index(string from, string to, string content)
        {
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
            currentContact.ContactMessages.Add(newMessage);
            await _context.SaveChangesAsync();
            return Json(currentContact.ContactMessages);
        }
    }
}
