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
        
        [Column("Role")]
        public string Role { get; set; }
        
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        

    }
}
