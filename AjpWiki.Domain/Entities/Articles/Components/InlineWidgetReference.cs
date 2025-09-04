using System;

namespace AjpWiki.Domain.Entities.Articles.Components
{
    public class InlineWidgetReference
    {
        // The token or shortcode as it appears in the markdown (e.g. "{{gallery:123}}" or "[widget:xyz]")
        public string? Token { get; set; }

        // Reference to a media asset or component id that implements the widget
        public Guid? MediaAssetId { get; set; }
        public Guid? ComponentId { get; set; }

        // Optional positional hints (for editors) like start index in the markdown source
        public int? StartIndex { get; set; }
    }
}
