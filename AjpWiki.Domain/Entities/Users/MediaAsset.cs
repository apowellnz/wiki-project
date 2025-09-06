namespace AjpWiki.Domain.Entities.Users
{
    public class MediaAsset
    {
        public Guid Id { get; set; }
        public Guid? OwnerUserId { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public long Size { get; set; }
        public string? StorageKey { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
