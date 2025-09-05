using System;

namespace AjpWiki.Application.Dto
{
    public record WikiArticleComponentDto(
        Guid Id,
        Guid ArticleId,
        string Type,
        string? Content
    );
}
