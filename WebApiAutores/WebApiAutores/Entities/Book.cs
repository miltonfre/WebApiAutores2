using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime? PublicationDate { get; set; }
        public List<Comment> Comments { get; set; }
        public List<AuthorBook> AuthorsBooks { get; set; }
    }
}
