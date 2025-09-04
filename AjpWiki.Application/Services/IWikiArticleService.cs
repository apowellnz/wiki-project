using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AjpWiki.Application.Dto;

namespace AjpWiki.Application.Services
{
    public interface IWikiArticleService
    {
    Task<WikiArticleDto?> GetAsync(Guid id);
    Task<IEnumerable<WikiArticleDto>> ListAsync();

    // Versioning
    Task<WikiArticleDto> CreateVersionAsync(Guid articleId, Guid authorId, bool isDraft, string? changeSummary, IEnumerable<object> components);
    Task PublishVersionAsync(Guid articleId, Guid versionId, Guid actorUserId);

    // Locking
    Task<bool> TryAcquireLockAsync(Guid articleId, Guid userId, TimeSpan lockTimeout);
    Task ReleaseLockAsync(Guid articleId, Guid userId);
    }
}
