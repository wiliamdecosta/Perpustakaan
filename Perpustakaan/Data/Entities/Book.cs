using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perpustakaan.Data.Entities
{
    [Table(name:"books")]
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name:"book_id")]
        public int Id{ get; set; }

        [Column(name:"title")]
        [Required]
        public  string Title{ get; set; }

        [Column(name:"author")]
        [Required]
        public string Author{ get; set; }

        [Column(name:"description")]
        public string Description{ get; set; }

        [Column(name:"published_date")]
        public DateTime PublishDate { get; set; }

        [Column(name: "created_date")]
        public DateTime CreatedDate { get; set; }

        [Column(name: "updated_date")]
        public DateTime UpdatedDate { get; set; }

        [Column(name:"created_by")]
        public string CreatedBy{ get; set; }

        [Column(name: "updated_by")]
        public string UpdatedBy { get; set; }

    }
}
