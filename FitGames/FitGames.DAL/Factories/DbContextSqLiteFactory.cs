using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace FitGames.DAL.Factories
{
    public class DbContextSqLiteFactory : IDbContextFactory<FitGamesDbContext>
    {
        private readonly DbContextOptionsBuilder<FitGamesDbContext> _contextOptionsBuilder = new();
        public DbContextSqLiteFactory(string databaseName)
        {
            _contextOptionsBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");
        }
        public FitGamesDbContext CreateDbContext()
        {
            return new FitGamesDbContext(_contextOptionsBuilder.Options);
        }
    }
}
