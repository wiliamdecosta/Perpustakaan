using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Perpustakaan.Data.Entities
{
    [Table(name:"endpoint")]
    public class EndpointPath
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name:"endpoint_id")]
        public int Id { get; set; }

        [Column(name:"path_route")]
        public string PathRoute { get; set; }

        [Column(name: "method")]
        public string HttpMethod { get; set; }

        [Column(name: "description")]
        public string? Description { get; set; }

        [Column(name: "created_date")]
        public DateTime CreatedDate { get; set; }

        [Column(name: "updated_date")]
        public DateTime? UpdatedDate { get; set; }

        [JsonIgnore]
        public virtual ICollection<EndpointRole>? EndpointRoles { get; set; } = new HashSet<EndpointRole>();
    }
}
