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
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<DeveloperEntity> Developers => Set<DeveloperEntity>();
}
