using System.ComponentModel.DataAnnotations;

namespace Perpustakaan.Models.Requests
{
    public class EndpointRequest
    {
        [Required(ErrorMessage = "Path Route dibutuhkan")]
        [MaxLength(500, ErrorMessage = "Maksimal karakter 500")]
        public string PathRoute { get; set; }

        [Required(ErrorMessage = "Method dibutuhkan")]
        [MaxLength(15, ErrorMessage = "Maksimal karakter 15")]
        public string HttpMethod { get; set; }

        public string Description { get; set; } = "";
    }
}
