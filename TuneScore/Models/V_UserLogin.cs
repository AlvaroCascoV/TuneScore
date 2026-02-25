
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuneScore.Models
{
    [Keyless]
    [Table("V_UserLogin")]
    public class V_UserLogin
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Username")]
        public string Username { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("PasswordPlain")]
        public string PasswordPlain { get; set; }

        [Column("PasswordHash")]
        public byte[] PasswordHash { get; set; }

        [Column("Salt")]
        public byte[] Salt { get; set; }
    }
}

