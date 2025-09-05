using System.Threading.Tasks;
using System.Collections.Generic;

namespace AjpWiki.Application.Services
{
    public interface INotificationService
    {
    Task SendNotificationAsync(Guid userId, string message);
    Task<IEnumerable<AjpWiki.Application.Dto.NotificationDto>> GetNotificationsAsync(Guid userId);
    Task<IEnumerable<AjpWiki.Application.Dto.NotificationDto>> GetNotificationsAsync(Guid userId, int page, int pageSize);
    Task<int> GetUnreadCountAsync(Guid userId);
    Task MarkAllAsReadAsync(Guid userId);
    }
}
