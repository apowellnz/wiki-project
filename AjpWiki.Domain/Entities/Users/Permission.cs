using System;
namespace AjpWiki.Domain.Entities.Users
{
    public class Permission
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
