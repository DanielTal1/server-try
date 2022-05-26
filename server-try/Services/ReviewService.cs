using server.Models;
using server.Services;
using System.Xml.Linq;

namespace server.Services
{

    public class ReviewService : IReviewService
    {
        private static List<Review> reviews = new List<Review>();
        public List<Review> GetAll()
        {
            return reviews;
        }
        public Review Get(int id)
        {
            return reviews.Find(x => x.Id == id);
        }
        public void Create(string name, string content, int rate)
        {
            int id = 0;
            if (reviews.Count > 0)
            {
                id = reviews.Max(x => x.Id) + 1;
            }
            DateTime now = DateTime.Now;
            string Date = now.ToString("g");
            reviews.Add(new Review() { Id = id, Name = name, Content = content, Rate = rate, Date = Date });
        }
        public void Edit(int id, string name, string content, int rate)
        {
            // CHANGE THE DATE TO THE NEW ONE
            DateTime now = DateTime.Now;
            string Date = "edited on " + now.ToString("g");

            Review review = Get(id);
            review.Name = name;
            review.Content = content;
            review.Rate = rate;
            review.Date = Date;
        }
        public void Delete(int id)
        {
            reviews.Remove(Get(id));
        }

    }

}
