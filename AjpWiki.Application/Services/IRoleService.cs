using System.Threading.Tasks;
using System.Collections.Generic;

namespace AjpWiki.Application.Services
{
    public interface IRoleService
    {
    Task AssignRoleAsync(Guid userId, string role);
    Task RemoveRoleAsync(Guid userId, string role);
    Task<IEnumerable<string>> GetUserRolesAsync(Guid userId);
        Task<IEnumerable<string>> GetAllRolesAsync();
    }
}
