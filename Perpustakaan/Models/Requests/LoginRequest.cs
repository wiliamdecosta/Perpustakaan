using System.ComponentModel.DataAnnotations;

namespace Perpustakaan.Models.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email dibutuhkan")]
        [EmailAddress(ErrorMessage = "Format email salah")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password dibutuhkan")]
        public string Password { get; set; }
    }
}
