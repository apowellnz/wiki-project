using System;
using AjpWiki.Domain.Entities.Articles;
namespace AjpWiki.Domain.Entities.Articles.Components
{
    public class TitleComponent : ArticleComponent
    {
    /// <summary>
    /// Semantic title block. Note: for prose and inline headings, prefer using Markdown inside a
    /// <see cref="AjpWiki.Domain.Entities.Articles.Components.RichTextComponent"/>. This component
    /// is useful when a heading needs to be a discrete block (for layout or reuse) rather than inline text.
    /// </summary>
    public string? Text { get; set; }
    public int Level { get; set; } = 1; // h1, h2, etc.
    }
}
