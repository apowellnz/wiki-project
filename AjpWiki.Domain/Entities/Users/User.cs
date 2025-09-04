using System;
using System.Collections.Generic;

namespace AjpWiki.Domain.Entities.Users
{
    // Aggregate root: User
    public class User
    {
        public Guid Id { get; set; }

        // Authentication
        public string? Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }

        // Profile
        public string? DisplayName { get; set; }
        public Guid? AvatarId { get; set; }

        // Security
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public List<UserRole> Roles { get; set; } = new();

        // Optional: linked avatar or media asset
        // public Avatar? Avatar { get; set; }
    }
}
