namespace AjpWiki.Domain.Entities.Articles
{
    /// <summary>
    /// Represents a wiki article root entity. Versioned content lives in <see cref="WikiArticleVersion"/> entities.
    /// </summary>
    public class WikiArticle
    {
        public Guid Id { get; set; }

        // Human-friendly title (may mirror a Title component in a version)
        public string? Title { get; set; }

        // URL slug; should be unique across articles
        public string? Slug { get; set; }

        // The id of the currently active (working) version. This can point to a draft the user is editing.
        public Guid? CurrentVersionId { get; set; }

        // The id of the last published version. Null if never published.
        public Guid? PublishedVersionId { get; set; }

        // Whether the article is currently locked for editing.
        public bool IsLocked { get; set; }

        // Optional user id of the user holding the lock.
        public Guid? LockedByUserId { get; set; }

        // When the lock was acquired (UTC)
        public DateTimeOffset? LockAcquiredAt { get; set; }

        // Metadata timestamps (stored as an absolute instant with offset)
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
