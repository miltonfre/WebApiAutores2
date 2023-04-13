using WebApiAutores.Entities;

namespace WebApiAutores.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public List<AutorDTO> Autors { get; set; }
       // public List<CommentDTO> Comments { get; set; }
    }
}
