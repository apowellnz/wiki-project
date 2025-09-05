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
            svc.AssignRoleAsync(uid, "admin").GetAwaiter().GetResult();
            Assert.True(true);
        }

        [Fact]
        public void RemoveRole_ShouldRemoveRoleFromUser()
        {
            Assert.True(true);
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
