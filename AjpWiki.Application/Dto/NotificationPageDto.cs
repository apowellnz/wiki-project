using System.Collections.Generic;

namespace AjpWiki.Application.Dto
{
    public record NotificationPageDto(
        IEnumerable<NotificationDto> Items,
        int TotalCount,
        int Page,
        int PageSize
    );
}
