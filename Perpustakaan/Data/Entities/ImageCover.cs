using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Perpustakaan.Data.Entities
{
    [Table(name: "image_cover")]
    public class ImageCover
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name: "image_cover_id")]
        public int Id { get; set; }

        [Column(name:"file_name")]
        public String FileName { get; set; }

        [Column(name: "original_file_name")]
        public String OriginalFileName { get; set; }

        [Column(name: "file_path")]
        public String FilePath { get; set; }


        [Column(name: "created_date")]
        public DateTime CreatedDate { get; set; }

        [Column(name: "updated_date")]
        public DateTime? UpdatedDate { get; set; }

        [Column(name: "created_by")]
        public string CreatedBy { get; set; }

        [Column(name: "updated_by")]
        public string UpdatedBy { get; set; }

        [Column(name: "book_id")]
        [ForeignKey("Book")]
        [JsonIgnore]
        public int BookId { get; set; }

        [JsonIgnore]
        public virtual Book Book { get; set; }
    }
}
