using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.DTOs
{
    public class BookCreationDTO
    {
        [Required]
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public List<int> AuthorsIds{ get; set; } 
    }
}
