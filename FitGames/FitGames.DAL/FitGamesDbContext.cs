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

        modelBuilder.Entity<GameEntity>()
            .HasMany(g => g.Libraries)
            .WithMany(l => l.Games)
            .UsingEntity<LibraryGameEntity>(
                "GameLibrary",
                j => j.HasOne(lg => lg.Library)
                      .WithMany(l => l.GameLibraries)
                      .HasForeignKey(lg => lg.LibraryId)
                      .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne(lg => lg.Game)
                      .WithMany(g => g.LibraryGames)
                      .HasForeignKey(lg => lg.GameId)
                      .OnDelete(DeleteBehavior.Cascade),
                j => j.HasKey(lg => new { lg.LibraryId, lg.GameId })
            );

        modelBuilder.Entity<GameEntity>()
            .HasOne(g => g.Developer)
            .WithMany(d => d.PublishedGames)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserEntity>()
            .HasOne(u => u.Library)
            .WithOne()
            .HasForeignKey<LibraryEntity>(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
