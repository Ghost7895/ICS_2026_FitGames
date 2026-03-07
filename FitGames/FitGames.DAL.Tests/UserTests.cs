using System.Net.Http.Headers;
using FitGames.DAL.Entities;
using FitGames.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace FitGames.DAL.Tests
{
    public class UserTests : DbContextTestsBase
    {
        private async Task<UserEntity> Create_And_Save_User_Entity(string username = "meno", string mail = "mail@amil",
            string? name = null, string? surname = null, string? phone = null)
        {
            LibraryEntity lib = new() { Name = "Test" };
            UserEntity user = new()
            {
                Username = username,
                Email = mail,
                Library = lib,
                Name = name,
                Surname = surname,
                PhoneNumber = phone
            };

            GameDbContextSut.Users.Add(user);
            await GameDbContextSut.SaveChangesAsync();

            return user;
        }
        [Fact]
        public async Task Add_New_User_Persisted()
        {
            // Arrange
            LibraryEntity lib = new() { Name = "Test" };
            const string name = "Test";
            const string mail = "Test@test";
            UserEntity user = new()
            {
                Username = name,
                Email = mail,
                Library = lib
            };

            // Act
            GameDbContextSut.Users.Add(user);
            await GameDbContextSut.SaveChangesAsync();

            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Users.Include(u => u.Library).First(u => u.Id == user.Id);
            Assert.Equal(user.Id, entityFromDb.Id);
            Assert.Equal(name, entityFromDb.Username);
            Assert.Equal(mail, entityFromDb.Email);
            Assert.Equal(lib.Id, entityFromDb.Library.Id);
        }

        [Fact]

        public async Task Add_New_User_With_Name_Persisted()
        {
            // Arrange
            LibraryEntity lib = new() { Name = "Test" };
            const string name = "achjaj";
            UserEntity user = new()
            {
                Username = "Test",
                Email = "Test@test",
                Library = lib,
                Name = name
            };

            // Act
            GameDbContextSut.Users.Add(user);
            await GameDbContextSut.SaveChangesAsync();

            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Users.Include(u => u.Library).First(u => u.Id == user.Id);
            Assert.Equal(name, entityFromDb.Name);
        }

        [Fact]
        public async Task Add_New_User_With_Surname_Persisted()
        {
            // Arrange
            LibraryEntity lib = new() { Name = "Test" };
            const string surname = "achjaj";
            UserEntity user = new()
            {
                Username = "Test",
                Email = "Test@test",
                Library = lib,
                Surname = surname
            };

            // Act
            GameDbContextSut.Users.Add(user);
            await GameDbContextSut.SaveChangesAsync();

            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Users.Include(u => u.Library).First(u => u.Id == user.Id);
            Assert.Equal(surname, entityFromDb.Surname);
        }


        [Fact]
        public async Task Add_New_User_With_Phone_Number_Persisted()
        {
            // Arrange
            LibraryEntity lib = new() { Name = "Test" };
            const string phoneNumber = "21354654";
            UserEntity user = new()
            {
                Username = "Test",
                Email = "Test@test",
                Library = lib,
                PhoneNumber = phoneNumber
            };

            // Act
            GameDbContextSut.Users.Add(user);
            await GameDbContextSut.SaveChangesAsync();

            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Users.Include(u => u.Library).First(u => u.Id == user.Id);
            Assert.Equal(phoneNumber, entityFromDb.PhoneNumber);
        }

        [Fact]
        public async Task Update_Username()
        {
            // Arrange
            const string newName = "novemeno";
            var user = await Create_And_Save_User_Entity();


            //act
            user.Username = newName;
            await GameDbContextSut.SaveChangesAsync();

            // assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Users.First(u => u.Id == user.Id);
            Assert.Equal(newName, entityFromDb.Username);
        }

        [Fact]
        public async Task Update_Email()
        {
            // Arrange
            const string newMail = "novemeno@asd";
            var user = await Create_And_Save_User_Entity();


            //act
            user.Email = newMail;
            await GameDbContextSut.SaveChangesAsync();

            // assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Users.First(u => u.Id == user.Id);
            Assert.Equal(newMail, entityFromDb.Email);
        }

        [Fact]
        public async Task Update_Name()
        {
            // Arrange
            const string newName = "novemeno";
            var user = await Create_And_Save_User_Entity();


            //act
            user.Name= newName;
            await GameDbContextSut.SaveChangesAsync();

            // assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Users.First(u => u.Id == user.Id);
            Assert.Equal(newName, entityFromDb.Name);
        }


        [Fact]

        public async Task Update_Surname()
        {
            // Arrange
            const string newSurname = "novemeno";
            var user = await Create_And_Save_User_Entity();


            //act
            user.Surname = newSurname;
            await GameDbContextSut.SaveChangesAsync();

            // assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Users.First(u => u.Id == user.Id);
            Assert.Equal(newSurname, entityFromDb.Surname);
        }

        [Fact]
        public async Task Update_Phone_Number()
        {
            // Arrange
            const string newPhone = "44564654";
            var user = await Create_And_Save_User_Entity();


            //act
            user.PhoneNumber = newPhone;
            await GameDbContextSut.SaveChangesAsync();

            // assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            var entityFromDb = dbx.Users.First(u => u.Id == user.Id);
            Assert.Equal(newPhone, entityFromDb.PhoneNumber);
        }


        [Fact]
        public async Task Remove_User_From_Db()
        {
            // Arrange
            LibraryEntity lib = new() { Name = "Test" };
            UserEntity user = new()
            {
                Username = "Test",
                Email = "Test@test",
                Library = lib
            };
            GameDbContextSut.Users.Add(user);
            await GameDbContextSut.SaveChangesAsync();

            // Act
            GameDbContextSut.Users.Remove(user);
            await GameDbContextSut.SaveChangesAsync();


            // Assert
            await using var dbx = base.DbContextFactory.CreateDbContext();
            Assert.DoesNotContain(dbx.Users, u => u.Id == user.Id);
            Assert.DoesNotContain(dbx.Libraries, u => u.Id == lib.Id);
        }
    }
}