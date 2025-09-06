using System;

namespace AjpWiki.Application.Dto
{
    public record WikiArticleDto(
        Guid Id,
        string? Title,
        string? Slug,
        Guid? CurrentVersionId,
        Guid? PublishedVersionId,
        bool IsLocked
    );
}
