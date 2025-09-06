namespace AjpWiki.Domain.Entities.Articles
{
    /// <summary>
    /// Represents a version (revision) of an article. Versions are immutable once created.
    /// They contain an ordered collection of components composing the page.
    /// </summary>
    public class WikiArticleVersion
    {
        public Guid Id { get; set; }

        // Parent article
        public Guid ArticleId { get; set; }

        // Author who created this version
        public Guid AuthorId { get; set; }

        // Indicates whether this version is a draft (only visible to the author or editors) or published
        public bool IsDraft { get; set; }

        // Free-text message about the change
        public string? ChangeSummary { get; set; }

        // When this version was created (instant + offset)
        public DateTimeOffset CreatedAt { get; set; }

        // Ordered list of page components (blocks)
        // Not mapped by EF in the test harness to keep model simple.
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public List<ArticleComponent> Components { get; set; } = new List<ArticleComponent>();
    }
}
