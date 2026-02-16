using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TuneScore.Models;

[Index("Username", Name = "UQ__Users__536C85E49F41945E", IsUnique = true)]
[Index("Email", Name = "UQ__Users__A9D10534A170B2FB", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [StringLength(50)]
    [Column("Username")]
    public string Username { get; set; } = null!;

    [StringLength(100)]
    [Column("Email")]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    [Column("PasswordHash")]
    public string PasswordHash { get; set; } = null!;

    [StringLength(20)]
    [Column("Role")]
    public string Role { get; set; } = null!;
    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
