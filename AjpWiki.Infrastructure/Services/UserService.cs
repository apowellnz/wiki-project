using AjpWiki.Application.Services;
using AjpWiki.Infrastructure.Data;
using AjpWiki.Domain.Entities.Users;

namespace AjpWiki.Infrastructure.Services
{
    public class UserService(WikiDbContext _db) : IUserService
    {

        public async Task CreateUserAsync(string name, string email, string password)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("email is required", nameof(email));
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6) throw new ArgumentException("password is too short", nameof(password));

            // Uniqueness check
            var exists = _db.Users.Any(u => u.Email == email);
            if (exists) throw new InvalidOperationException("A user with that email already exists");

            var user = new User { Id = Guid.NewGuid(), DisplayName = name, Email = email, PasswordHash = password, CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow };
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

    public async Task DeleteAccountAsync(Guid userId)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return;

        // Remove related notifications
        var notes = _db.Notifications.Where(n => n.UserId == userId).ToList();
        if (notes.Any()) _db.Notifications.RemoveRange(notes);

        // Optionally, handle articles (not deleting articles by default)

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
    }
        public Task ResetPasswordAsync(string email) => Task.CompletedTask;

    public Task ChangeAvatarAsync(Guid userId, byte[] avatarData)
    {
        // Validate avatar: max 2MB for now
        const int maxBytes = 2 * 1024 * 1024;
        if (avatarData == null || avatarData.Length == 0) throw new ArgumentException("avatar data required", nameof(avatarData));
        if (avatarData.Length > maxBytes) throw new ArgumentException("avatar too large", nameof(avatarData));

        // For now we don't persist avatars; in future store in blob store and save AvatarId on user
        return Task.CompletedTask;
    }
    }
}
