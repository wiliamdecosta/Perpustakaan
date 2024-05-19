using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Perpustakaan.Data.Entities
{
    [Table(name:"roles")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name:"role_id")]
        public int Id { get; set; }

        [Column(name:"name")]
        public string Name { get; set; }

        [Column(name: "created_date")]
        public DateTime CreatedDate { get; set; }

        [Column(name: "updated_date")]
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<UserRole>? UserRole { get; set; }

        [JsonIgnore]
        public virtual ICollection<EndpointRole>? EndpointRoles { get; set; }

    }
}
