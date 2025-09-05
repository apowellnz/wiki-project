using System.Threading.Tasks;
using System.Collections.Generic;
using AjpWiki.Application.Services;

namespace AjpWiki.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        public Task AssignRoleAsync(int userId, string role) => Task.CompletedTask;
        public Task RemoveRoleAsync(int userId, string role) => Task.CompletedTask;
        public Task<IEnumerable<string>> GetUserRolesAsync(int userId) => Task.FromResult<IEnumerable<string>>(new List<string>());
        public Task<IEnumerable<string>> GetAllRolesAsync() => Task.FromResult<IEnumerable<string>>(new List<string>());
    }
}
