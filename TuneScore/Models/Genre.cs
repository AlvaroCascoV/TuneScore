using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TuneScore.Models;

[Index("Name", Name = "UQ__Genres__737584F60BCCF99C", IsUnique = true)]
public partial class Genre
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [StringLength(50)]
    [Column("Name")]
    public string Name { get; set; } = null!;

    [InverseProperty("Genre")]
    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
