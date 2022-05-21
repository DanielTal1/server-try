using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Content { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rate { get; set; }
        public string Date { get; set; }
    }
}
