using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Services;
using server_try.Data;

namespace server.Controllers
{
    public class ReviewsController : Controller
    {
        private IReviewService _service;

        public ReviewsController()
        {
            _service = new ReviewService();
            if (_service.GetAll().Count == 0)
            {
                _service.Create("Rita", "The best one!", 5);
                _service.Create("Daniel", "Cool app guys!", 5);
                _service.Create("Foo", "Awful", 1);
            }
        }

        // GET: Reviews
        public IActionResult Index()
        {
            return View(_service.GetAll());
        }

        [HttpPost]
        public IActionResult Index(string query)
        {
            if (query == null)
            {
                return RedirectToAction(nameof(Index));
            }
            //var q = from review in _service.GetAll()
            //        where review.Name.Contains(query) ||
            //              review.Content.Contains(query)
            //        select review;
            //return View(nameof(Index), q.ToList());
            List<Review> reviews = _service.GetAll();
            if (reviews == null)
            {
                return View(_service.GetAll());
            }
            var q = _service.GetAll().Where(review => review.Name.ToLower().Contains(query) ||
                                                      review.Content.ToLower().Contains(query));
            return View(q.ToList());
        }

        // GET: Reviews/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = _service.Get((int)id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Content,Rate,Date")] Review review)
        {
            if (ModelState.IsValid)
            {
                _service.Create(review.Name, review.Content, review.Rate);
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        // GET: Reviews/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = _service.Get((int)id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Content,Rate,Date")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _service.Edit(id, review.Name, review.Content, review.Rate);
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = _service.Get((int)id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var review = _service.Get((int)id);
            if (review != null)
            {
                _service.Delete(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
