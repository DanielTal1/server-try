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
    public class MessagesController : Controller
    {
        private readonly server_tryContext _context;

        public MessagesController(server_tryContext context)
        {
            _context = context;
        }

        [HttpGet("api/contacts/:{id}/messages")]
        public async Task<IActionResult> Get(string id,string user)
        {
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                //couldnt find user
                return Json("couldnt find user");
            }
            var currentContact = await  _context.Contact.Include(x => x.ContactMessages).FirstOrDefaultAsync(u => u.id == id && u.UserId == currentUser.Id);
            if (currentContact == null)
            {
                return Json("couldnt find contact");
            }
            return Json(currentContact.ContactMessages);
        }

        [HttpPost("api/contacts/:{id}/messages")]
        public async Task<IActionResult> Post(string id, [FromBody] Dictionary<string, string> data)
        {
            string content = data["content"];
            string user = data["user"];
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                return Json("couldnt find user");
            }
            var currentContact = currentUser.ContactsList.Where(u => u.id == id).FirstOrDefault();
            if (currentContact == null)
            {
                return Json("couldnt find contact");
            }
            var newMessage = new Message(content, true);
            currentContact.ContactMessages.Insert(0,newMessage);
            currentContact.last = content;
            currentContact.lastdate= DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
            await _context.SaveChangesAsync();
            return Json(currentContact.ContactMessages);
        }

        [HttpGet("api/contacts/:{id}/messages/:{id2}")]
        public async Task<IActionResult> Get(string id, string user, int id2)
        {
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                return Json("couldnt find user");
            }
            var currentContact = await _context.Contact.Include(x => x.ContactMessages).FirstOrDefaultAsync(u => u.id == id && u.UserId == currentUser.Id);
            if (currentUser == null)
            {
                return Json("couldnt find contact");
            }
            var askedMessage= currentContact.ContactMessages.Where(m=>m.id==id2).FirstOrDefault();
            if (askedMessage == null)
            {
                return Json("couldnt find message");
            }
            return Json(askedMessage);

        }


        [HttpPut("api/contacts/:{id}/messages/:{id2}")]
        public async Task<IActionResult> Put(string id, int id2, [FromBody] Dictionary<string, string> data)
        {
            string content = data["content"];
            string user = data["user"];
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                return Json("couldnt find user");
            }
            var currentContact = await _context.Contact.Include(x => x.ContactMessages).FirstOrDefaultAsync(u => u.id == id && u.UserId == currentUser.Id);
            if (currentUser == null)
            {
                return Json("couldnt find contact");
            }
            var askedMessage = currentContact.ContactMessages.Where(m => m.id == id2).FirstOrDefault();
            if (askedMessage == null)
            {
                return Json("couldnt find message");
            }
            askedMessage.content = content;
            _context.Message.Update(askedMessage);
            await _context.SaveChangesAsync();
            return Json(askedMessage);

        }


        [HttpDelete("api/contacts/:{id}/messages/:{id2}")]
        public async Task<IActionResult> Delete(string id, [FromBody] string user, int id2)
        {
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                return Json("couldnt find user");
            }
            var currentContact = await _context.Contact.Include(x => x.ContactMessages).FirstOrDefaultAsync(u => u.id == id && u.UserId == currentUser.Id);
            if (currentUser == null)
            {
                return Json("couldnt find contact");
            }
            var askedMessage = currentContact.ContactMessages.FirstOrDefault(m => m.id == id2);
            if (askedMessage == null)
            {
                return Json("couldnt find message");
            }
            _context.Message.Remove(askedMessage);
            await _context.SaveChangesAsync();
            return Json("success");

        }
        
    }
}
