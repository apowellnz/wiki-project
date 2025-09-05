using System.Threading.Tasks;
using System.Collections.Generic;
using AjpWiki.Application.Services;
using AjpWiki.Infrastructure.Data;
using AjpWiki.Domain.Entities.Notifications;
using System;
using System.Linq;

namespace AjpWiki.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly WikiDbContext _db;

        public NotificationService(WikiDbContext db)
        {
            _db = db;
        }

        public async Task SendNotificationAsync(Guid userId, string message)
        {
            // Verify user exists - policy: fail fast if recipient doesn't exist
            var userExists = _db.Users.Find(userId) != null;
            if (!userExists) throw new InvalidOperationException("User not found");

            var n = new Notification { Id = Guid.NewGuid(), UserId = userId, Message = message, IsRead = false, CreatedAt = DateTimeOffset.UtcNow };
            await _db.Notifications.AddAsync(n);
            await _db.SaveChangesAsync();
        }

        public Task<IEnumerable<string>> GetNotificationsAsync(Guid userId)
        {
            var list = _db.Notifications.Where(x => x.UserId == userId).OrderByDescending(n => n.CreatedAt).Select(x => x.Message).ToList();
            return Task.FromResult<IEnumerable<string>>(list);
        }

        public Task<IEnumerable<string>> GetNotificationsAsync(Guid userId, int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var skip = (page - 1) * pageSize;
            var list = _db.Notifications.Where(x => x.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => x.Message)
                .ToList();

            return Task.FromResult<IEnumerable<string>>(list);
        }

        public async Task MarkAsReadAsync(Guid notificationId)
        {
            var n = await _db.Notifications.FindAsync(notificationId);
            if (n == null) return; // idempotent: missing -> no-op
            if (n.IsRead) return; // idempotent
            n.IsRead = true;
            await _db.SaveChangesAsync();
        }
    }
}
