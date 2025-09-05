using System.Threading.Tasks;
using System.Collections.Generic;
using AjpWiki.Application.Services;

namespace AjpWiki.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        public Task SendNotificationAsync(int userId, string message) => Task.CompletedTask;
        public Task<IEnumerable<string>> GetNotificationsAsync(int userId) => Task.FromResult<IEnumerable<string>>(new List<string>());
    }
}
