using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TuneScore.Models;

[Index("SongId", Name = "IX_Ratings_SongId")]
[Index("UserId", Name = "IX_Ratings_UserId")]
[Index("UserId", "SongId", Name = "UQ_User_Song", IsUnique = true)]
public partial class Rating
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("UserId")]
    public int UserId { get; set; }

    [Column("SongId")]
    public int SongId { get; set; }

    [Column("Score")]
    public int Score { get; set; }

    [StringLength(100)]
    [Column("Comment")]
    public string? Comment { get; set; }

    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }

    [Column("UpdatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("SongId")]
    [InverseProperty("Ratings")]
    public virtual Song Song { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Ratings")]
    public virtual User User { get; set; } = null!;
}
