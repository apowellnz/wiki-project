using System.Threading.Tasks;
using System.Collections.Generic;

namespace AjpWiki.Application.Services
{
    public interface IUserService
    {
        Task CreateUserAsync(string name, string email, string password);
        Task RequestAccessAsync(string email);
    Task ApproveUserAsync(Guid userId);
    Task RejectUserAsync(Guid userId);
    Task EditProfileAsync(Guid userId, string name, string email, string password);
    Task ChangeAvatarAsync(Guid userId, byte[] avatarData);
    Task DeleteAccountAsync(Guid userId);
        Task ResetPasswordAsync(string email);
    }
}
