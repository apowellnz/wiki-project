using System;

namespace AjpWiki.Domain.Entities.Notifications
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
