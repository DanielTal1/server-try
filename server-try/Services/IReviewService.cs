using server.Models;

namespace server.Services
{
    public interface IReviewService
    {

        public List<Review> GetAll();
        public Review Get(int id);
        public void Create(string name, string content, int rate);
        public void Edit(int id, string name, string content, int rate);
        public void Delete(int id);
    }
}
