using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Perpustakaan.Data.Entities
{
    [Table(name:"role_endpoint")]
    public class EndpointRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name: "role_endpoint_id")]
        public int Id { get; set; }

        [Column(name: "role_id")]
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        
        [Column(name:"endpoint_id")]
        [ForeignKey("EndpointPath")]
        public int EndpointId { get; set; }


        [Column(name: "created_date")]
        public DateTime CreatedDate { get; set; }

        [Column(name: "updated_date")]
        public DateTime? UpdatedDate { get; set; }

        public virtual Role Role { get; }

        public virtual EndpointPath EndpointPath { get; }


    }
}
