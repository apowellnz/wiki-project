using System.Threading.Tasks;
using System.Collections.Generic;
using AjpWiki.Application.Services;
using AjpWiki.Infrastructure.Data;
using AjpWiki.Domain.Entities.Users;
using System;
using System.Linq;

namespace AjpWiki.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly WikiDbContext _db;

        public RoleService(WikiDbContext db)
        {
            _db = db;
        }

        public async Task AssignRoleAsync(Guid userId, string role)
        {
            var ur = new UserRole { Id = Guid.NewGuid(), UserId = userId, Role = role };
            await _db.AddAsync(ur);
            await _db.SaveChangesAsync();
        }

    public Task RemoveRoleAsync(Guid userId, string role) => Task.CompletedTask;

        public Task<IEnumerable<string>> GetUserRolesAsync(Guid userId)
        {
            var list = _db.Set<UserRole>().Where(r => r.UserId == userId).Select(r => r.Role).ToList();
            return Task.FromResult<IEnumerable<string>>(list);
        }

        public Task<IEnumerable<string>> GetAllRolesAsync() => Task.FromResult<IEnumerable<string>>(new string[0]);
    }
}
