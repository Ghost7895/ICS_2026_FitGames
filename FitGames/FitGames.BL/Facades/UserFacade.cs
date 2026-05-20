using FitGames.BL.Facades.Interfaces;
using FitGames.BL.Mappers;
using FitGames.BL.Models;
using FitGames.DAL.Entities;
using FitGames.DAL.Mappers;
using FitGames.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace FitGames.BL.Facades;

public class UserFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    UserModelMapper modelMapper)
    : FacadeBase<UserEntity, UserListModel, UserDetailModel, UserEntityMapper>(unitOfWorkFactory, modelMapper),
        IUserFacade
{
    protected override ICollection<string> IncludesNavigationPathDetail =>
        new[] { nameof(UserEntity.Library) };

    public async Task<UserDetailModel?> GetUserByUsernameAsync(string username)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var entities = await uow.GetRepository<UserEntity, UserEntityMapper>()
                                .GetAllAsync(
                                    filter: u => u.Username == username,
                                    includePaths: new[] { nameof(UserEntity.Library) });

        return ModelMapper.MapToDetailModel(entities.SingleOrDefault());
    }

    public override async Task<UserDetailModel> SaveAsync(UserDetailModel model)
    {
        // 1. Ak user už existuje (má nejaké ID), použijeme klasický Save
        if (model.Id != Guid.Empty)
        {
            return await base.SaveAsync(model);
        }

        // 2. Ak vytvárame nového usera, spravíme vlastnú logiku
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        // Prevod do entity
        UserEntity userEntity = ModelMapper.MapToEntity(model);
        userEntity.Id = Guid.NewGuid();

        // Vytvorenie dedikovanej knižnice pre tohto usera
        LibraryEntity newLibrary = new LibraryEntity
        {
            Id = Guid.NewGuid(),
            Name = $"{userEntity.Username}'s Library",
            UserId = userEntity.Id
        };

        var userRepo = uow.GetRepository<UserEntity, UserEntityMapper>();
        var libraryRepo = uow.GetRepository<LibraryEntity, LibraryEntityMapper>();

        // Obe entity pošleme do testovacej transakcie
        await libraryRepo.InsertAsync(newLibrary);
        await userRepo.InsertAsync(userEntity);

        // V JEDNOM momente sa obe entity uložia. Ak niečo padne, neuloží sa ani jedna.
        await uow.CommitAsync();

        // Mapovanie späť pre UI
        return ModelMapper.MapToDetailModel(userEntity);
    }
}
