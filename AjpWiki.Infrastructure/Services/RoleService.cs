using AjpWiki.Application.Services;
using AjpWiki.Infrastructure.Data;
using AjpWiki.Domain.Entities.Users;

namespace AjpWiki.Infrastructure.Services
{
    public class RoleService(WikiDbContext _db) : IRoleService
    {

        public async Task AssignRoleAsync(Guid callerUserId, Guid userId, string role)
        {
            // Only allow assigning 'admin' if caller is already admin
            if (role == "admin")
            {
                var callerIsAdmin = _db.Set<UserRole>().Any(r => r.UserId == callerUserId && r.Role == "admin");
                if (!callerIsAdmin) throw new UnauthorizedAccessException("Caller not permitted to assign admin role");
            }

            // Idempotent: don't add if role already present
            var exists = _db.Set<UserRole>().Any(r => r.UserId == userId && r.Role == role);
            if (exists) return;

            var ur = new UserRole { Id = Guid.NewGuid(), UserId = userId, Role = role };
            await _db.AddAsync(ur);
            await _db.SaveChangesAsync();
        }

    public async Task RemoveRoleAsync(Guid callerUserId, Guid userId, string role)
    {
        // For now, allow callers to remove roles; privileged removal could be gated similarly to Assign
        var ur = _db.Set<UserRole>().FirstOrDefault(r => r.UserId == userId && r.Role == role);
        if (ur == null) return; // no-op
        _db.Set<UserRole>().Remove(ur);
        await _db.SaveChangesAsync();
    }

        public Task<IEnumerable<string>> GetUserRolesAsync(Guid userId)
        {
            var list = _db.Set<UserRole>().Where(r => r.UserId == userId).Select(r => r.Role).ToList();
            return Task.FromResult<IEnumerable<string>>(list);
        }

        public Task<IEnumerable<string>> GetAllRolesAsync() => Task.FromResult<IEnumerable<string>>(new string[0]);
    }
}
