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
    public class ContactsController : Controller
    {
        private readonly server_tryContext _context;

        public ContactsController(server_tryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string user)
        {
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.UserName == user);
            if (currentUser == null)
            {
                return NotFound();
            }
            return Json(currentUser.ContactsList);
        }

        // POST: Contacts
        [HttpPost]
        public async Task<IActionResult> Index( string id, string name, string server, string user)
        {
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

        // GET: Contacts/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id, string user)
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

            return Json(contact);
        }



        [HttpPut]
        public async Task<IActionResult> Details2(string id,string name,string server, string user)
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


        [HttpDelete]
        public async Task<IActionResult> Details3(string id, string user)
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

            var contact = await _context.Contact.Include(x => x.ContactMessages).FirstOrDefaultAsync(u => u.id == id && u.UserId == currentUser.Id);
            if (contact == null)
            {
                return Json("contact wasnt found");
            }
            //contact.ContactMessages.Clear();
            _context.Contact.Remove(contact);
            await _context.SaveChangesAsync();
            return Json("success");
        }

        /*
        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,server,last,lastdate")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }



        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Contact == null)
            {
                return Problem("Entity set 'server_tryContext.Contact'  is null.");
            }
            var contact = await _context.Contact.FindAsync(id);
            if (contact != null)
            {
                _context.Contact.Remove(contact);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(string id)
        {
          return (_context.Contact?.Any(e => e.id == id)).GetValueOrDefault();
        }
        */
    }
}
