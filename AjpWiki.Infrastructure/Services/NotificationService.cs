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

        public Task<IEnumerable<AjpWiki.Application.Dto.NotificationDto>> GetNotificationsAsync(Guid userId)
        {
            var list = _db.Notifications.Where(x => x.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(x => new AjpWiki.Application.Dto.NotificationDto(x.Id, x.UserId, x.Message, x.IsRead, x.CreatedAt))
                .ToList();

            return Task.FromResult<IEnumerable<AjpWiki.Application.Dto.NotificationDto>>(list);
        }

    public Task<IEnumerable<AjpWiki.Application.Dto.NotificationDto>> GetNotificationsAsync(Guid userId, int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var skip = (page - 1) * pageSize;
            var list = _db.Notifications.Where(x => x.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new AjpWiki.Application.Dto.NotificationDto(x.Id, x.UserId, x.Message, x.IsRead, x.CreatedAt))
                .ToList();

            return Task.FromResult<IEnumerable<AjpWiki.Application.Dto.NotificationDto>>(list);
        }

        public async Task MarkAsReadAsync(Guid notificationId)
        {
            var n = await _db.Notifications.FindAsync(notificationId);
            if (n == null) return; // idempotent: missing -> no-op
            if (n.IsRead) return; // idempotent
            n.IsRead = true;
            await _db.SaveChangesAsync();
        }

        public Task<int> GetUnreadCountAsync(Guid userId)
        {
            var count = _db.Notifications.Count(n => n.UserId == userId && !n.IsRead);
            return Task.FromResult(count);
        }

        public async Task MarkAllAsReadAsync(Guid userId)
        {
            var items = _db.Notifications.Where(n => n.UserId == userId && !n.IsRead).ToList();
            if (!items.Any()) return;
            foreach (var n in items) n.IsRead = true;
            await _db.SaveChangesAsync();
        }
    }
}
