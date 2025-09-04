using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AjpWiki.Domain.Entities.Articles;
using AjpWiki.Domain.Repositories;

namespace AjpWiki.Infrastructure.Repositories
{
    public class InMemoryWikiArticleRepository : IWikiArticleRepository
    {
        private readonly List<WikiArticle> _store = new()
        {
            new WikiArticle { Id = Guid.NewGuid(), Title = "Welcome" }
        };
        private readonly List<WikiArticleVersion> _versions = new();

    // Simple in-memory lock tracking (articleId -> (userId, acquiredAt))
    private readonly Dictionary<Guid, (Guid UserId, DateTimeOffset AcquiredAt)> _locks = new();

        public Task<WikiArticle?> GetByIdAsync(Guid id) => Task.FromResult(_store.FirstOrDefault(x => x.Id == id));

        public Task<IEnumerable<WikiArticle>> ListAsync() => Task.FromResult<IEnumerable<WikiArticle>>(_store);

        public Task<WikiArticleVersion> CreateVersionAsync(WikiArticleVersion version)
        {
            if (version.Id == Guid.Empty) version.Id = Guid.NewGuid();
            version.CreatedAt = version.CreatedAt == default ? DateTimeOffset.UtcNow : version.CreatedAt;
            _versions.Add(version);

            var article = _store.FirstOrDefault(a => a.Id == version.ArticleId);
            if (article != null)
            {
                article.CurrentVersionId = version.Id;
                article.UpdatedAt = DateTimeOffset.UtcNow;
            }

            return Task.FromResult(version);
        }

        public Task<WikiArticleVersion?> GetVersionAsync(Guid versionId)
        {
            return Task.FromResult(_versions.FirstOrDefault(v => v.Id == versionId));
        }

        public Task<IEnumerable<WikiArticleVersion>> ListVersionsAsync(Guid articleId)
        {
            var list = _versions.Where(v => v.ArticleId == articleId).OrderByDescending(v => v.CreatedAt).ToList();
            return Task.FromResult<IEnumerable<WikiArticleVersion>>(list);
        }

        public Task PublishVersionAsync(Guid articleId, Guid versionId, Guid actorUserId)
        {
            var article = _store.FirstOrDefault(a => a.Id == articleId);
            if (article == null) throw new InvalidOperationException("Article not found");

            var version = _versions.FirstOrDefault(v => v.Id == versionId && v.ArticleId == articleId);
            if (version == null) throw new InvalidOperationException("Version not found");

            version.IsDraft = false;
            article.PublishedVersionId = versionId;
            article.UpdatedAt = DateTimeOffset.UtcNow;

            return Task.CompletedTask;
        }

        public Task<bool> TryAcquireLockAsync(Guid articleId, Guid userId, TimeSpan lockTimeout)
        {
            var now = DateTimeOffset.UtcNow;
            if (!_locks.TryGetValue(articleId, out var current))
            {
                _locks[articleId] = (userId, now);
                var article = _store.FirstOrDefault(a => a.Id == articleId);
                if (article != null)
                {
                    article.IsLocked = true;
                    article.LockedByUserId = userId;
                    article.LockAcquiredAt = now;
                }
                return Task.FromResult(true);
            }

            // If existing lock expired, replace it
        if (current.AcquiredAt + lockTimeout <= now)
            {
                _locks[articleId] = (userId, now);
                var article = _store.FirstOrDefault(a => a.Id == articleId);
                if (article != null)
                {
                    article.IsLocked = true;
                    article.LockedByUserId = userId;
            article.LockAcquiredAt = now;
                }
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task ReleaseLockAsync(Guid articleId, Guid userId)
        {
            if (_locks.TryGetValue(articleId, out var current) && current.UserId == userId)
            {
                _locks.Remove(articleId);
                var article = _store.FirstOrDefault(a => a.Id == articleId);
                if (article != null)
                {
                    article.IsLocked = false;
                    article.LockedByUserId = null;
                    article.LockAcquiredAt = null;
                }
            }

            return Task.CompletedTask;
        }
    }
}
