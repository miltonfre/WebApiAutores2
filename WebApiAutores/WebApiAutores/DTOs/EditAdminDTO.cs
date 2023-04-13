using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.DTOs
{
    public class EditAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
