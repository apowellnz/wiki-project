using System.Threading.Tasks;
using System.Collections.Generic;

namespace AjpWiki.Application.Services
{
    public interface IRoleService
    {
        Task AssignRoleAsync(int userId, string role);
        Task RemoveRoleAsync(int userId, string role);
        Task<IEnumerable<string>> GetUserRolesAsync(int userId);
        Task<IEnumerable<string>> GetAllRolesAsync();
    }
}
