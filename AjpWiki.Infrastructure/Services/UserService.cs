using System.Threading.Tasks;
using System.Collections.Generic;
using AjpWiki.Application.Services;
using AjpWiki.Infrastructure.Data;
using AjpWiki.Domain.Entities.Users;
using System;

namespace AjpWiki.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly WikiDbContext _db;

        public UserService(WikiDbContext db)
        {
            _db = db;
        }

        public async Task CreateUserAsync(string name, string email, string password)
        {
            var user = new User { Id = Guid.NewGuid(), DisplayName = name, Email = email, PasswordHash = password, CreatedAt = DateTimeOffset.UtcNow };
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public Task RequestAccessAsync(string email)
        {
            // Out of scope: would send admin notification / create request record
            return Task.CompletedTask;
        }

        public Task ApproveUserAsync(Guid userId) => Task.CompletedTask;
        public Task RejectUserAsync(Guid userId) => Task.CompletedTask;

        public async Task EditProfileAsync(Guid userId, string name, string email, string password)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null) return;
            user.DisplayName = name;
            user.Email = email;
            user.PasswordHash = password;
            user.UpdatedAt = DateTimeOffset.UtcNow;
            await _db.SaveChangesAsync();
        }

    public Task ChangeAvatarAsync(Guid userId, byte[] avatarData) => Task.CompletedTask;
    public Task DeleteAccountAsync(Guid userId) => Task.CompletedTask;
        public Task ResetPasswordAsync(string email) => Task.CompletedTask;
    }
}
