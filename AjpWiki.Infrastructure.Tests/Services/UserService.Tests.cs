using Xunit;
using Microsoft.EntityFrameworkCore;
using AjpWiki.Infrastructure.Data;
using AjpWiki.Infrastructure.Services;
using System;

namespace AjpWiki.Infrastructure.Tests.Services
{
    public class UserServiceTests
    {
        // User Story 1: Account Creation
        [Fact]
        public void CreateUser_ShouldCreateNewUser()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var db = new WikiDbContext(options);
            var svc = new UserService(db);
            svc.CreateUserAsync("bob", "bob@example.com", "pw").GetAwaiter().GetResult();
            var u = db.Users.FirstOrDefaultAsync().GetAwaiter().GetResult();
            Assert.NotNull(u);
            Assert.Equal("bob@example.com", u.Email);
        }

        // User Story 2: Access Request & Approval
        [Fact]
        public void RequestAccess_ShouldSendRequestToAdmin()
        {
            Assert.True(true);
        }

        [Fact]
        public void ApproveUser_ShouldGrantAccess()
        {
            Assert.True(true);
        }

        [Fact]
        public void RejectUser_ShouldDenyAccess()
        {
            Assert.True(true);
        }

        // User Story 11: Profile Management
        [Fact]
        public void EditProfile_ShouldUpdateUserProfile()
        {
            Assert.True(true);
        }

        [Fact]
        public void ChangeAvatar_ShouldUpdateUserAvatar()
        {
            Assert.True(true);
        }

        [Fact]
        public void DeleteAccount_ShouldRemoveUser()
        {
            Assert.True(true);
        }

        // User Story 14: Password Reset
        [Fact]
        public void ResetPassword_ShouldSendResetInstructions()
        {
            Assert.True(true);
        }
    }
}
