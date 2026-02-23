using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TuneScore.Models;

[Index("ArtistId", Name = "IX_Albums_ArtistId")]
public partial class Album
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [StringLength(150)]
    [Column("Title")]
    public string Title { get; set; } = null!;
    [Column("ReleaseYear")]
    public int ReleaseYear { get; set; }
    [Column("ArtistId")]
    public int ArtistId { get; set; }
    [StringLength(300)]
    [Column("ImageName")]
    public string? ImageName { get; set; }
    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("ArtistId")]
    [InverseProperty("Albums")]
    public virtual Artist Artist { get; set; } = null!;

    [InverseProperty("Album")]
    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
