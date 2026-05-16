using FitGames.app.Services.Interfaces;
using FitGames.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitGames.app.Services
{
    public class UserContext : IUserContext
    {
        public Guid? CurrentUserId { get; set; }
        public Guid? CurrentLibraryId { get; set; }
        public string? Username { get; set; }

        public void Initialize(UserEntity user)
        {
            CurrentUserId = user.Id;
            CurrentLibraryId = user.Library.Id;
            Username = user.Username;
        }
    }
}