using System;

namespace AjpWiki.Domain.Entities.Users
{
    // lightweight reference to a stored avatar; storage details are handled by infrastructure
    public class Avatar
    {
        public Guid Id { get; set; }
        public Guid OwnerUserId { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public long Size { get; set; }
        public string? StorageKey { get; set; } // abstract key (could be DB blob id or blob path/url)
        public DateTimeOffset CreatedAt { get; set; }
    }
}
