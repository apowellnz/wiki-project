using System;

namespace AjpWiki.Domain.Entities
{
    public class WikiArticleRevision
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public Guid AuthorId { get; set; }
        public string? BodyMarkdown { get; set; }
        public string? BodyHtml { get; set; }
        public string? ChangeSummary { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
