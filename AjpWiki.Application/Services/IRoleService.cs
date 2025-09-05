using System.Threading.Tasks;
using System.Collections.Generic;

namespace AjpWiki.Application.Services
{
    public interface IRoleService
    {
    // callerUserId: the actor performing the role assignment (used for permission checks)
    Task AssignRoleAsync(Guid callerUserId, Guid userId, string role);
    Task RemoveRoleAsync(Guid callerUserId, Guid userId, string role);
    Task<IEnumerable<string>> GetUserRolesAsync(Guid userId);
        Task<IEnumerable<string>> GetAllRolesAsync();
    }
}
