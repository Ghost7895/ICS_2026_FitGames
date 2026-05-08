using System;
using System.Collections.Generic;
using System.Text;
using FitGames.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitGames.DAL;


public class FitGamesDbContext(DbContextOptions<FitGamesDbContext> options) : DbContext(options)
{
    public DbSet<GameEntity> Games => Set<GameEntity>();
    public DbSet<LibraryEntity> Libraries => Set<LibraryEntity>();
    public DbSet<LibraryGameEntity> GameLibraries => Set<LibraryGameEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<DeveloperEntity> Developers => Set<DeveloperEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // This handles the many-to-many relationship AND the join table configuration in one go
        modelBuilder.Entity<GameEntity>()
            .HasMany(g => g.Libraries)
            .WithMany(l => l.Games)
            .UsingEntity<LibraryGameEntity>(
                l => l.HasOne(lg => lg.Library).WithMany(e => e.GameLibraries).HasForeignKey(lg => lg.LibraryId),
                r => r.HasOne(lg => lg.Game).WithMany(e => e.LibraryGames).HasForeignKey(lg => lg.GameId),
                j =>
                {
                    j.HasKey(lg => new { lg.LibraryId, lg.GameId });
                    j.ToTable("GameLibraries");
                });

        // Developer relationship
        modelBuilder.Entity<GameEntity>()
            .HasOne(g => g.Developer)
            .WithMany(d => d.PublishedGames)
            .HasForeignKey(g => g.DeveloperId)
            .OnDelete(DeleteBehavior.Restrict);

        // User/Library relationship
        modelBuilder.Entity<UserEntity>()
            .HasOne(u => u.Library)
            .WithOne()
            .HasForeignKey<LibraryEntity>(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
