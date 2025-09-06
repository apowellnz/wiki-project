using System;
using System.Text;
using System.Text.RegularExpressions;

namespace AjpWiki.Application.Utils
{
    public static class SlugHelper
    {
        // Generate a simple URL-friendly slug from a title.
        public static string Generate(string? title)
        {
            if (string.IsNullOrWhiteSpace(title)) return string.Empty;
            var normalized = title.ToLowerInvariant().Trim();
            // Replace non-letter/digit with hyphen
            normalized = Regex.Replace(normalized, "[^a-z0-9]+", "-");
            // Trim hyphens
            normalized = normalized.Trim('-');
            // Collapse multiple hyphens
            normalized = Regex.Replace(normalized, "-+", "-");
            return normalized;
        }
    }
}
