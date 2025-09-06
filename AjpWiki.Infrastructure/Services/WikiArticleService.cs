using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AjpWiki.Application.Dto;
using AjpWiki.Application.Mappings;
using AjpWiki.Application.Utils;
using AjpWiki.Application.Exceptions;
using AjpWiki.Application.Services;
using AjpWiki.Domain.Entities.Articles;
using AjpWiki.Domain.Repositories;

namespace AjpWiki.Infrastructure.Services
{
    public class WikiArticleService(IWikiArticleRepository _repo) : IWikiArticleService
    {

        public async Task<WikiArticleDto> CreateArticleAsync(WikiArticleDto articleDto)
        {
            var id = articleDto.Id == Guid.Empty ? Guid.NewGuid() : articleDto.Id;
            var slug = articleDto.Slug;
            if (string.IsNullOrWhiteSpace(slug)) slug = SlugHelper.Generate(articleDto.Title);

            var entity = new WikiArticle { Id = id, Title = articleDto.Title, Slug = slug, CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow };
            try
            {
                var created = await _repo.CreateArticleAsync(entity);
                return created.ToDto();
            }
            catch (InvalidOperationException ex) when (ex.Message?.Contains("slug", StringComparison.InvariantCultureIgnoreCase) == true)
            {
                throw new DuplicateSlugException(ex.Message);
            }
        }

        public async Task<WikiArticleDto> EditArticleAsync(Guid articleId, WikiArticleDto articleDto)
        {
            var article = await _repo.GetByIdAsync(articleId) ?? throw new InvalidOperationException("Article not found");
            article.Title = articleDto.Title;
            article.UpdatedAt = DateTimeOffset.UtcNow;
            // For now, persist via repository by creating a new version to track changes
            var version = new WikiArticleVersion { ArticleId = article.Id, AuthorId = Guid.NewGuid(), IsDraft = false, ChangeSummary = "edit via service" };
            await _repo.CreateVersionAsync(version);
            return article.ToDto();
        }

        public Task<IEnumerable<WikiArticleDto>> SearchArticlesAsync(string query) => Task.FromResult<IEnumerable<WikiArticleDto>>(new List<WikiArticleDto>());
        public Task<IEnumerable<WikiArticleDto>> GetArticlesByTagAsync(string tag) => Task.FromResult<IEnumerable<WikiArticleDto>>(new List<WikiArticleDto>());

        public async Task<WikiArticleDto?> GetArticleByIdAsync(Guid articleId)
        {
            var a = await _repo.GetByIdAsync(articleId);
            return a?.ToDto();
        }

        public async Task<IEnumerable<WikiArticleDto>> GetArticleHistoryAsync(Guid articleId)
        {
            var versions = await _repo.ListVersionsAsync(articleId);
            var article = await _repo.GetByIdAsync(articleId);
            return versions.Select(v => article!.ToDto());
        }

        public async Task RollbackArticleAsync(Guid articleId, Guid versionId)
        {
            var version = await _repo.GetVersionAsync(versionId) ?? throw new InvalidOperationException("Version not found");
            // Simple rollback: create a new version that mirrors the chosen version
            var newVersion = new WikiArticleVersion
            {
                ArticleId = articleId,
                AuthorId = version.AuthorId,
                IsDraft = false,
                ChangeSummary = "rollback",
                CreatedAt = DateTimeOffset.UtcNow,
                Components = version.Components
            };
            await _repo.CreateVersionAsync(newVersion);
        }

        public Task AddComponentAsync(Guid articleId, WikiArticleComponentDto componentDto) => Task.CompletedTask;
        public Task<IEnumerable<WikiArticleComponentDto>> GetComponentsAsync(Guid articleId) => Task.FromResult<IEnumerable<WikiArticleComponentDto>>(new List<WikiArticleComponentDto>());
        public Task ProposeChangeAsync(Guid articleId, WikiArticleDto proposedChangeDto) => Task.CompletedTask;
        public Task ReviewChangeAsync(Guid articleId, Guid changeId, bool accept) => Task.CompletedTask;
    }
}
