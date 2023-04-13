using System.ComponentModel.DataAnnotations;
using WebApiAutores.CustomValidators;

namespace WebApiAutores.DTOs
{
    public class AutorCreationDTO
    {
        [Required(ErrorMessage = "{0} is requiered")]
        [StringLength(50, ErrorMessage = "{0} should be less than {1} chars")]
        [FirstLetterCapitalize]
        public string Name { get; set; }
    }
}
