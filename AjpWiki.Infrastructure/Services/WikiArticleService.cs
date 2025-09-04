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
        private readonly IWikiArticleRepository _repo;

        public WikiArticleService(IWikiArticleRepository repo)
        {
            _repo = repo;
        }

        public async Task<WikiArticleDto?> GetAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity?.ToDto();
        }

        public async Task<IEnumerable<WikiArticleDto>> ListAsync()
        {
            var list = await _repo.ListAsync();
            return list.Select(x => x.ToDto());
        }

        public async Task<WikiArticleDto> CreateVersionAsync(Guid articleId, Guid authorId, bool isDraft, string? changeSummary, IEnumerable<object> components)
        {
            // Map incoming lightweight component representations to domain components.
            var domainComponents = new List<ArticleComponent>();
            int order = 0;
            foreach (var c in components ?? Array.Empty<object>())
            {
                order++;
                // Very small mapping: if caller passes an anonymous object with a Type property, attempt map
                if (c is IDictionary<string, object> dict && dict.TryGetValue("type", out var t))
                {
                    var typeStr = t?.ToString();
                    if (string.Equals(typeStr, "title", StringComparison.OrdinalIgnoreCase))
                    {
                        var comp = new AjpWiki.Domain.Entities.Articles.Components.TitleComponent { Order = order, Type = ComponentType.Title };
                        if (dict.TryGetValue("text", out var text)) comp.Text = text?.ToString();
                        domainComponents.Add(comp);
                        continue;
                    }
                    if (string.Equals(typeStr, "richtext", StringComparison.OrdinalIgnoreCase))
                    {
                        var comp = new AjpWiki.Domain.Entities.Articles.Components.RichTextComponent { Order = order, Type = ComponentType.RichText };
                        if (dict.TryGetValue("markdown", out var md)) comp.Markdown = md?.ToString();
                        domainComponents.Add(comp);
                        continue;
                    }
                    if (string.Equals(typeStr, "image", StringComparison.OrdinalIgnoreCase))
                    {
                        var comp = new AjpWiki.Domain.Entities.Articles.Components.ImageComponent { Order = order, Type = ComponentType.Image };
                        if (dict.TryGetValue("mediaAssetId", out var ma) && Guid.TryParse(ma?.ToString(), out var mid)) comp.MediaAssetId = mid;
                        if (dict.TryGetValue("alt", out var alt)) comp.AltText = alt?.ToString();
                        domainComponents.Add(comp);
                        continue;
                    }
                }

                // Fallback: custom component wrapper
                var custom = new AjpWiki.Domain.Entities.Articles.Components.CustomComponent { Order = order, Type = ComponentType.Custom };
                domainComponents.Add(custom);
            }

            var version = new WikiArticleVersion
            {
                ArticleId = articleId,
                AuthorId = authorId,
                IsDraft = isDraft,
                ChangeSummary = changeSummary,
                Components = domainComponents
            };

            var created = await _repo.CreateVersionAsync(version);
            var article = await _repo.GetByIdAsync(articleId) ?? throw new InvalidOperationException("Article not found");
            return article.ToDto();
        }

        public async Task PublishVersionAsync(Guid articleId, Guid versionId, Guid actorUserId)
        {
            await _repo.PublishVersionAsync(articleId, versionId, actorUserId);
        }

        public Task<bool> TryAcquireLockAsync(Guid articleId, Guid userId, TimeSpan lockTimeout) => _repo.TryAcquireLockAsync(articleId, userId, lockTimeout);

        public Task ReleaseLockAsync(Guid articleId, Guid userId) => _repo.ReleaseLockAsync(articleId, userId);
    }
}
