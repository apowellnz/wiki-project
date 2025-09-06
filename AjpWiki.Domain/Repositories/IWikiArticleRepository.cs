using AjpWiki.Domain.Entities.Articles;

namespace AjpWiki.Domain.Repositories
{
    public interface IWikiArticleRepository
    {
    // Create an article
    Task<WikiArticle> CreateArticleAsync(WikiArticle article);
        Task<WikiArticle?> GetByIdAsync(Guid id);
        Task<IEnumerable<WikiArticle>> ListAsync();

        // Version operations
        Task<WikiArticleVersion> CreateVersionAsync(WikiArticleVersion version);
        Task<WikiArticleVersion?> GetVersionAsync(Guid versionId);
        Task<IEnumerable<WikiArticleVersion>> ListVersionsAsync(Guid articleId);

        // Publish a version (sets PublishedVersionId on article)
        Task PublishVersionAsync(Guid articleId, Guid versionId, Guid actorUserId);

        // Lock/Unlock
        Task<bool> TryAcquireLockAsync(Guid articleId, Guid userId, TimeSpan lockTimeout);
        Task ReleaseLockAsync(Guid articleId, Guid userId);
    }
}
