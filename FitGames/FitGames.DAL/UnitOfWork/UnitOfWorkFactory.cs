using FitGames.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;


namespace FitGames.DAL.UnitOfWork;

public class UnitOfWorkFactory(IDbContextFactory<FitGamesDbContext> dbContextFactory) : IUnitOfWorkFactory
{
    public IUnitOfWork Create() => new UnitOfWork(dbContextFactory.CreateDbContext());
}