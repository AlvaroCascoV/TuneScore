using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TuneScore.Models;

public partial class Artist
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [StringLength(100)]
    [Column("Name")]
    public string Name { get; set; } = null!;
    [StringLength(300)]
    [Column("ImageUrl")]
    public string? ImageUrl { get; set; }
    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("Artist")]
    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
}
