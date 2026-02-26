using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TuneScore.Models;

namespace TuneScore.Data;

public partial class TuneScoreContext : DbContext
{

    public TuneScoreContext(DbContextOptions<TuneScoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<V_UserLogin> V_UserLogin { get; set; }

    public virtual DbSet<UserSalt> UserSalts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configuration is provided via dependency injection in Program.cs.
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Albums__3214EC07B8837BA3");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums).HasConstraintName("FK_Albums_Artists");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Artists__3214EC078C1BB6CD");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genres__3214EC07A1B5F34D");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ratings__3214EC076FB0DF19");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Song).WithMany(p => p.Ratings).HasConstraintName("FK_Ratings_Songs");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings).HasConstraintName("FK_Ratings_Users");
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Songs__3214EC072AF1BF27");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Album).WithMany(p => p.Songs).HasConstraintName("FK_Songs_Albums");

            entity.HasOne(d => d.Genre).WithMany(p => p.Songs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Songs_Genres");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC079ABE2BA9");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Role).HasDefaultValue("User");
        });

        modelBuilder.Entity<V_UserLogin>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("V_UserLogin");
        });

        modelBuilder.Entity<UserSalt>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(us => us.User)
                .WithOne(u => u.UserSalt)
                .HasForeignKey<UserSalt>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserSalts_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
