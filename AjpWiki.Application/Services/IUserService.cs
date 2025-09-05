using System.Threading.Tasks;
using System.Collections.Generic;

namespace AjpWiki.Application.Services
{
    public interface IUserService
    {
        Task CreateUserAsync(string name, string email, string password);
        Task RequestAccessAsync(string email);
        Task ApproveUserAsync(int userId);
        Task RejectUserAsync(int userId);
        Task EditProfileAsync(int userId, string name, string email, string password);
        Task ChangeAvatarAsync(int userId, byte[] avatarData);
        Task DeleteAccountAsync(int userId);
        Task ResetPasswordAsync(string email);
    }
}
