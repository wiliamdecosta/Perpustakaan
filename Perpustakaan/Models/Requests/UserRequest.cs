using System.ComponentModel.DataAnnotations;

namespace Perpustakaan.Models.Requests
{
    public class UserRequest
    {
        [Required(ErrorMessage = "Nama dibutuhkan")]
        [MaxLength(100, ErrorMessage = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email dibutuhkan")]
        [EmailAddress(ErrorMessage ="Format email salah")]
        [MaxLength(100, ErrorMessage = "Maximal karakter email 255")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password dibutuhkan")]
        [MinLength(6, ErrorMessage ="Minimal 6 karakter password")]
        [MaxLength(16, ErrorMessage ="Maksimal 16 karakter password")]
        public string Password { get; set; }

    }
}
