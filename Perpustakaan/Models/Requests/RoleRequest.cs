using System.ComponentModel.DataAnnotations;

namespace Perpustakaan.Models.Requests
{
    public class RoleRequest
    {
        [Required(ErrorMessage = "Nama dibutuhkan")]
        [MaxLength(100, ErrorMessage = "Name")]
        public string Name { get; set; }
    }
}
