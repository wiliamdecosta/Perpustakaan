using System.ComponentModel.DataAnnotations;

namespace Perpustakaan.Models.Requests
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage ="Token dibutuhkan")]
        public string Token { get; set; }
        [Required(ErrorMessage = "Refresh token dibutuhkan")]
        public string RefreshToken { get; set; }
    }
}
