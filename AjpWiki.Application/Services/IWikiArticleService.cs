using AjpWiki.Application.Dto;

namespace AjpWiki.Application.Services
{
    public interface IWikiArticleService
    {
        // Only service contract, no implementation or out-of-scope details
        Task<object> CreateArticleAsync(object articleDto);
        Task<object> EditArticleAsync(int articleId, object articleDto);
        Task<IEnumerable<object>> SearchArticlesAsync(string query);
        Task<IEnumerable<object>> GetArticlesByTagAsync(string tag);
        Task<object> GetArticleByIdAsync(int articleId);
        Task<IEnumerable<object>> GetArticleHistoryAsync(int articleId);
        Task RollbackArticleAsync(int articleId, int versionId);
        Task AddComponentAsync(int articleId, object componentDto);
        Task<IEnumerable<object>> GetComponentsAsync(int articleId);
        Task ProposeChangeAsync(int articleId, object proposedChangeDto);
        Task ReviewChangeAsync(int articleId, int changeId, bool accept);
    }
}
