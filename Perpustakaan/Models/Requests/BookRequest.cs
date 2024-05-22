using System.ComponentModel.DataAnnotations;

namespace Perpustakaan.Models.Requests
{
    public class BookRequest
    {
        [Required(ErrorMessage = "Judul dibutuhkan")]
        [MaxLength(255, ErrorMessage = "Maksimal karakter 255")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author dibutuhkan")]
        [MaxLength(255, ErrorMessage = "Maximal karakter Details 255")]
        public string Author { get; set; }

        public string Description { get; set; }

        public DateTime PublishDate {  get; set; }

        public List<IFormFile> ImageCovers { get; set; }
    }
}
