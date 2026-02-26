using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuneScore.Models
{
    [Table("Users")]
    public class Register
    {
        [Key]
        [Column("Id")]
        public int IdUser { get; set; }


        [Column("Username")]
        [Required]
        public string Username { get; set; }

        [Column("Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Column("PasswordPlain")]
        [Required]
        public string PasswordPlain { get; set; }

        /// <summary>Optional; DB default is 'User' (see TuneScoreDB.sql).</summary>
        [Column("Role")]
        public string? Role { get; set; }

        /// <summary>Optional; DB default is GETDATE() (see TuneScoreDB.sql).</summary>
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        

    }
}
