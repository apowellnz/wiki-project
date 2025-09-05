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
            var n = new Notification { Id = Guid.NewGuid(), UserId = userId, Message = message, IsRead = false, CreatedAt = DateTimeOffset.UtcNow };
            await _db.Notifications.AddAsync(n);
            await _db.SaveChangesAsync();
        }

        public Task<IEnumerable<string>> GetNotificationsAsync(Guid userId)
        {
            var list = _db.Notifications.Where(x => x.UserId == userId).Select(x => x.Message).ToList();
            return Task.FromResult<IEnumerable<string>>(list);
        }
    }
}
