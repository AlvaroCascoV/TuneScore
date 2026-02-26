using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TuneScore.Models
{
    [Table("UserSalts")]
    public class UserSalt
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }

        [Column("PasswordHash")]
        public byte[] PasswordHash { get; set; } = null!;

        [Column("Salt")]
        public string Salt { get; set; } = null!;

        [ForeignKey("UserId")]
        [InverseProperty("UserSalt")]
        [JsonIgnore]
        public virtual User User { get; set; } = null!;
    }
}

