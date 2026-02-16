using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TuneScore.Models;

namespace TuneScore.Models;

public partial class Song
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [StringLength(150)]
    [Column("Title")]
    public string Title { get; set; } = null!;

    [Column("DurationSeconds")]
    public int? DurationSeconds { get; set; }

    [Column("AlbumId")]
    public int AlbumId { get; set; }

    [Column("GenreId")]
    public int GenreId { get; set; }

    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("AlbumId")]
    [InverseProperty("Songs")]
    public virtual Album Album { get; set; } = null!;

    [ForeignKey("GenreId")]
    [InverseProperty("Songs")]
    public virtual Genre Genre { get; set; } = null!;

    [InverseProperty("Song")]
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
