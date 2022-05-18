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
    public class MessagesController : Controller
    {
        private readonly server_tryContext _context;

        public MessagesController(server_tryContext context)
        {
            _context = context;
        }

        // GET: Messages
        public async Task<IActionResult> Index(string id,string user)
        {
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                return Json("couldnt find user");
            }
            var currentContact = await  _context.Contact.Include(x => x.ContactMessages).FirstOrDefaultAsync(u => u.id == id && u.UserId == currentUser.Id);
            if (currentContact == null)
            {
                return Json("couldnt find contact");
            }
            return Json(currentContact.ContactMessages);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string id, string user,string content)
        {
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                return Json("couldnt find user");
            }
            var currentContact = _context.Contact.Include(x => x.ContactMessages).Where(u => u.id == id && u.UserId==currentUser.Id).FirstOrDefault();
            if (currentUser == null)
            {
                return Json("couldnt find contact");
            }
            var newMessage = new Message(content, true);
            currentContact.ContactMessages.Add(newMessage);
            await _context.SaveChangesAsync();
            return Json(currentContact.ContactMessages);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, string user, int id2)
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


        [HttpPut]
        public async Task<IActionResult> Details2(string id, string user, int id2,string content)
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




        [HttpDelete]
        public async Task<IActionResult> Details3(string id, string user, int id2)
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
            var askedMessage = currentContact.ContactMessages.Where(m => m.id == id2).FirstOrDefault();
            if (askedMessage == null)
            {
                return Json("couldnt find message");
            }
            _context.Message.Remove(askedMessage);
            await _context.SaveChangesAsync();
            return Json("success");

        }
        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,content,created,sent")] Message message)
        {
            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Message == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,content,created,sent")] Message message)
        {
            if (id != message.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Message == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .FirstOrDefaultAsync(m => m.id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Message == null)
            {
                return Problem("Entity set 'server_tryContext.Message'  is null.");
            }
            var message = await _context.Message.FindAsync(id);
            if (message != null)
            {
                _context.Message.Remove(message);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
          return (_context.Message?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
