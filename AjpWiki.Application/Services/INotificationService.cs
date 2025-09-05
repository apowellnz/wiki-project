using System.Threading.Tasks;
using System.Collections.Generic;

namespace AjpWiki.Application.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(int userId, string message);
        Task<IEnumerable<string>> GetNotificationsAsync(int userId);
    }
}
