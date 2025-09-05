using System.Threading.Tasks;
using System.Collections.Generic;
using AjpWiki.Application.Services;

namespace AjpWiki.Infrastructure.Services
{
    public class UserService : IUserService
    {
        public Task CreateUserAsync(string name, string email, string password) => Task.CompletedTask;
        public Task RequestAccessAsync(string email) => Task.CompletedTask;
        public Task ApproveUserAsync(int userId) => Task.CompletedTask;
        public Task RejectUserAsync(int userId) => Task.CompletedTask;
        public Task EditProfileAsync(int userId, string name, string email, string password) => Task.CompletedTask;
        public Task ChangeAvatarAsync(int userId, byte[] avatarData) => Task.CompletedTask;
        public Task DeleteAccountAsync(int userId) => Task.CompletedTask;
        public Task ResetPasswordAsync(string email) => Task.CompletedTask;
    }
}
