namespace AjpWiki.Domain.Entities.Users
{
    public class Role
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }

        public List<RolePermission> Permissions { get; set; } = new();
        public List<UserRole> Users { get; set; } = new();
    }
}
