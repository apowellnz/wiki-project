using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AjpWiki.Domain.Entities.Articles;
using AjpWiki.Domain.Repositories;
using AjpWiki.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AjpWiki.Infrastructure.Repositories
{
    public class EfWikiArticleRepository : IWikiArticleRepository
    {
        private readonly WikiDbContext _db;

        public EfWikiArticleRepository(WikiDbContext db)
        {
            _db = db;
        }

        public async Task<WikiArticle> CreateArticleAsync(WikiArticle article)
        {
            if (article.Id == Guid.Empty) article.Id = Guid.NewGuid();
            article.CreatedAt = DateTimeOffset.UtcNow;
            article.UpdatedAt = article.CreatedAt;
            await _db.WikiArticles.AddAsync(article);
            await _db.SaveChangesAsync();
            return article;
        }

        public Task<WikiArticle?> GetByIdAsync(Guid id) => _db.WikiArticles.FindAsync(id).AsTask().ContinueWith(t => t.Result);

        public Task<IEnumerable<WikiArticle>> ListAsync() => Task.FromResult<IEnumerable<WikiArticle>>(_db.WikiArticles.AsNoTracking().ToList());

        public async Task<WikiArticleVersion> CreateVersionAsync(WikiArticleVersion version)
        {
            if (version.Id == Guid.Empty) version.Id = Guid.NewGuid();
            version.CreatedAt = version.CreatedAt == default ? DateTimeOffset.UtcNow : version.CreatedAt;
            await _db.WikiArticleVersions.AddAsync(version);
            var article = await _db.WikiArticles.FindAsync(version.ArticleId);
            if (article != null)
            {
                article.CurrentVersionId = version.Id;
                article.UpdatedAt = DateTimeOffset.UtcNow;
            }
            await _db.SaveChangesAsync();
            return version;
        }

        public Task<WikiArticleVersion?> GetVersionAsync(Guid versionId) => _db.WikiArticleVersions.FindAsync(versionId).AsTask().ContinueWith(t => t.Result);

        public Task<IEnumerable<WikiArticleVersion>> ListVersionsAsync(Guid articleId)
        {
            var list = _db.WikiArticleVersions.Where(v => v.ArticleId == articleId).OrderByDescending(v => v.CreatedAt).ToList();
            return Task.FromResult<IEnumerable<WikiArticleVersion>>(list);
        }

        public async Task PublishVersionAsync(Guid articleId, Guid versionId, Guid actorUserId)
        {
            var article = await _db.WikiArticles.FindAsync(articleId) ?? throw new InvalidOperationException("Article not found");
            var version = await _db.WikiArticleVersions.FindAsync(versionId);
            if (version == null || version.ArticleId != articleId) throw new InvalidOperationException("Version not found");
            version.IsDraft = false;
            article.PublishedVersionId = versionId;
            article.UpdatedAt = DateTimeOffset.UtcNow;
            await _db.SaveChangesAsync();
        }

        public Task<bool> TryAcquireLockAsync(Guid articleId, Guid userId, TimeSpan lockTimeout)
        {
            var article = _db.WikiArticles.Find(articleId);
            if (article == null) return Task.FromResult(false);
            if (!article.IsLocked || (article.LockAcquiredAt.HasValue && article.LockAcquiredAt.Value + lockTimeout <= DateTimeOffset.UtcNow))
            {
                article.IsLocked = true;
                article.LockedByUserId = userId;
                article.LockAcquiredAt = DateTimeOffset.UtcNow;
                _db.SaveChanges();
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task ReleaseLockAsync(Guid articleId, Guid userId)
        {
            var article = _db.WikiArticles.Find(articleId);
            if (article != null && article.LockedByUserId == userId)
            {
                article.IsLocked = false;
                article.LockedByUserId = null;
                article.LockAcquiredAt = null;
                _db.SaveChanges();
            }
            return Task.CompletedTask;
        }
    }
}
