using System;

namespace AjpWiki.Domain.Entities.Articles.Components
{
    public class RichTextComponent : AjpWiki.Domain.Entities.Articles.ArticleComponent
    {
        // Prefer storing a canonical markdown source and optionally precomputed HTML
    // Markdown is the canonical source of truth for the component content. Renderers should
    // parse Markdown and optionally expand shortcodes/placeholders for widget references.
    public string? Markdown { get; set; }

    // Optional precomputed HTML for fast rendering (can be null and regenerated from Markdown).
    public string? Html { get; set; }

    // Optional inline widget/placeholders discovered during editing or parsing. Each entry
    // maps a shortcode or placeholder token inside the Markdown to an internal component or media id.
    public System.Collections.Generic.List<InlineWidgetReference> InlineWidgetReferences { get; set; } = new();
    }
}
