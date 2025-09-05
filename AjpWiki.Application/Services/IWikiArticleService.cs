using AjpWiki.Application.Dto;

namespace AjpWiki.Application.Services
{
    public interface IWikiArticleService
    {
    Task<WikiArticleDto> CreateArticleAsync(WikiArticleDto articleDto);
    Task<WikiArticleDto> EditArticleAsync(Guid articleId, WikiArticleDto articleDto);
    Task<IEnumerable<WikiArticleDto>> SearchArticlesAsync(string query);
    Task<IEnumerable<WikiArticleDto>> GetArticlesByTagAsync(string tag);
    Task<WikiArticleDto?> GetArticleByIdAsync(Guid articleId);
    Task<IEnumerable<WikiArticleDto>> GetArticleHistoryAsync(Guid articleId);
    Task RollbackArticleAsync(Guid articleId, Guid versionId);
    Task AddComponentAsync(Guid articleId, WikiArticleComponentDto componentDto);
    Task<IEnumerable<WikiArticleComponentDto>> GetComponentsAsync(Guid articleId);
    Task ProposeChangeAsync(Guid articleId, WikiArticleDto proposedChangeDto);
    Task ReviewChangeAsync(Guid articleId, Guid changeId, bool accept);
    }
}
