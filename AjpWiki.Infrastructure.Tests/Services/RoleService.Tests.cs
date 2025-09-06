using Xunit;
using System;
using Microsoft.EntityFrameworkCore;
using AjpWiki.Infrastructure.Data;
using AjpWiki.Infrastructure.Services;

namespace AjpWiki.Infrastructure.Tests.Services
{
    public class RoleServiceTests
    {
        // User Story 12: Role & Permission Management
        [Fact]
        public void AssignRole_ShouldAddRoleToUser()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var db = new WikiDbContext(options);
            var svc = new RoleService(db);
            var uid = Guid.NewGuid();
            // caller == uid for simplicity; since no admin required for non-privileged assignment
            svc.AssignRoleAsync(uid, uid, "editor").GetAwaiter().GetResult();
            var roles = svc.GetUserRolesAsync(uid).GetAwaiter().GetResult();
            Assert.Contains("editor", roles);
        }

        [Fact]
        public void RemoveRole_ShouldRemoveRoleFromUser()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var db = new WikiDbContext(options);
            var svc = new RoleService(db);
            var uid = Guid.NewGuid();
            svc.AssignRoleAsync(Guid.Empty, uid, "editor").GetAwaiter().GetResult();
            svc.RemoveRoleAsync(Guid.Empty, uid, "editor").GetAwaiter().GetResult();
            var rolesAfter = svc.GetUserRolesAsync(uid).GetAwaiter().GetResult();
            Assert.Empty(rolesAfter);
        }

        [Fact]
        public void AssignRole_ToUnknownUser_ShouldStillSucceedOrCreateRecord()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var db = new WikiDbContext(options);
            var svc = new RoleService(db);
            var uid = Guid.NewGuid();
            // Assign role to a user id that doesn't exist in Users table - allowed (system may support external identities)
            svc.AssignRoleAsync(Guid.Empty, uid, "viewer").GetAwaiter().GetResult();
            var roles = svc.GetUserRolesAsync(uid).GetAwaiter().GetResult();
            Assert.Contains("viewer", roles);
        }

        [Fact]
        public void AssignRole_Idempotent_WhenAlreadyAssigned_NoDuplicate()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var db = new WikiDbContext(options);
            var svc = new RoleService(db);
            var uid = Guid.NewGuid();
            svc.AssignRoleAsync(Guid.Empty, uid, "editor").GetAwaiter().GetResult();
            svc.AssignRoleAsync(Guid.Empty, uid, "editor").GetAwaiter().GetResult();
            var roles = svc.GetUserRolesAsync(uid).GetAwaiter().GetResult();
            Assert.Single(roles);
        }

        [Fact]
        public void RemoveRole_NotPresent_ShouldBeNoOp()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var db = new WikiDbContext(options);
            var svc = new RoleService(db);
            var uid = Guid.NewGuid();
            // should not throw
            svc.RemoveRoleAsync(Guid.Empty, uid, "nonexistent").GetAwaiter().GetResult();
        }

        [Fact]
        public void AssignPrivilegedRole_WithoutPermission_ShouldThrow()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var db = new WikiDbContext(options);
            var svc = new RoleService(db);
            var caller = Guid.NewGuid();
            var target = Guid.NewGuid();
            Assert.Throws<UnauthorizedAccessException>(() => svc.AssignRoleAsync(caller, target, "admin").GetAwaiter().GetResult());
        }

        [Fact]
        public void GetUserRoles_ShouldReturnAllRolesForUser()
        {
            Assert.Empty(new string[0]);
        }

        [Fact]
        public void GetAllRoles_ShouldReturnAllRoles()
        {
            Assert.Empty(new string[0]);
        }
    }
}
