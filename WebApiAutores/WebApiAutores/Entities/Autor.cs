using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.CustomValidators;

namespace WebApiAutores.Entities
{
    public class Autor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is requiered")]
        [StringLength(50, ErrorMessage ="{0} should be less than {1} chars" )]
        [FirstLetterCapitalize]
        public string Name { get; set; }
        [Range(18,120)]
        [NotMapped]//no tener en cuenta para la BD
        public int Age { get; set; }

        public List<AuthorBook> AuthorsBooks { get; set; }

    }
}
