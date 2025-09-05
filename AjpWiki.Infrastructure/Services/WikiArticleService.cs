using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AjpWiki.Application.Dto;
using AjpWiki.Application.Mappings;
using AjpWiki.Application.Services;
using AjpWiki.Domain.Entities.Articles;
using AjpWiki.Domain.Repositories;

namespace AjpWiki.Infrastructure.Services
{
    public class WikiArticleService : IWikiArticleService
    {
        public Task<WikiArticleDto> CreateArticleAsync(WikiArticleDto articleDto) => Task.FromResult(articleDto);
        public Task<WikiArticleDto> EditArticleAsync(Guid articleId, WikiArticleDto articleDto) => Task.FromResult(articleDto);
        public Task<IEnumerable<WikiArticleDto>> SearchArticlesAsync(string query) => Task.FromResult<IEnumerable<WikiArticleDto>>(new List<WikiArticleDto>());
        public Task<IEnumerable<WikiArticleDto>> GetArticlesByTagAsync(string tag) => Task.FromResult<IEnumerable<WikiArticleDto>>(new List<WikiArticleDto>());
        public Task<WikiArticleDto?> GetArticleByIdAsync(Guid articleId) => Task.FromResult<WikiArticleDto?>(null);
        public Task<IEnumerable<WikiArticleDto>> GetArticleHistoryAsync(Guid articleId) => Task.FromResult<IEnumerable<WikiArticleDto>>(new List<WikiArticleDto>());
        public Task RollbackArticleAsync(Guid articleId, Guid versionId) => Task.CompletedTask;
        public Task AddComponentAsync(Guid articleId, WikiArticleComponentDto componentDto) => Task.CompletedTask;
        public Task<IEnumerable<WikiArticleComponentDto>> GetComponentsAsync(Guid articleId) => Task.FromResult<IEnumerable<WikiArticleComponentDto>>(new List<WikiArticleComponentDto>());
        public Task ProposeChangeAsync(Guid articleId, WikiArticleDto proposedChangeDto) => Task.CompletedTask;
        public Task ReviewChangeAsync(Guid articleId, Guid changeId, bool accept) => Task.CompletedTask;
    }
}
