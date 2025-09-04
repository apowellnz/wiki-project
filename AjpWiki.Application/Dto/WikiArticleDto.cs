using System;

namespace AjpWiki.Application.Dto
{
    public record WikiArticleDto(
        Guid Id,
        string? Title,
        Guid? CurrentVersionId,
        Guid? PublishedVersionId,
        bool IsLocked
    );
}
