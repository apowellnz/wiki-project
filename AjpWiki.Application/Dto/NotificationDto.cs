using System;

namespace AjpWiki.Application.Dto
{
    public record NotificationDto(Guid Id, Guid UserId, string Message, bool IsRead, DateTimeOffset CreatedAt);
}
