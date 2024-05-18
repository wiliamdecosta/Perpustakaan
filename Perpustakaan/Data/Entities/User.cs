using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Perpustakaan.Data.Entities
{
    [Table(name:"users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name: "user_id")]
        public int Id{ get; set; }

        [Column(name:"name")]
        public string Name{ get; set; }

        [Column(name:"email")]
        public string Email{ get; set; }

        [Column(name:"password")]
        [JsonIgnore]
        public string Password{ get; set; }

        [Column(name: "created_date")]
        public DateTime CreatedDate  { get; set; }

        [Column(name: "updated_date")]
        public DateTime? UpdatedDate { get; set; }


        [Column(name: "refresh_token")]
        [JsonIgnore]
        public string? RefreshToken { get; set; }

        [Column(name: "refresh_token_expiry_time")]
        [JsonIgnore]
        public DateTime? RefreshTokenExpiryTime { get; set; }

    }
}
