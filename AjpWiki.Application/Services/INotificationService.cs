using System.Threading.Tasks;
using System.Collections.Generic;

namespace AjpWiki.Application.Services
{
    public interface INotificationService
    {
    Task SendNotificationAsync(Guid userId, string message);
    Task<IEnumerable<string>> GetNotificationsAsync(Guid userId);
    }
}
