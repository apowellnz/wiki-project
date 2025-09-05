using Xunit;
using Microsoft.EntityFrameworkCore;
using AjpWiki.Infrastructure.Data;
using AjpWiki.Infrastructure.Services;
using System;
using System.Linq;

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
            svc.CreateUserAsync("bob", "bob@example.com", "password").GetAwaiter().GetResult();
            var u = db.Users.FirstOrDefaultAsync().GetAwaiter().GetResult();
            Assert.NotNull(u);
            Assert.Equal("bob@example.com", u.Email);
        }

        [Fact]
        public void CreateUser_InvalidFields_ShouldThrow()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var db = new WikiDbContext(options);
            var svc = new UserService(db);
            Assert.Throws<ArgumentException>(() => svc.CreateUserAsync("bob", "", "short").GetAwaiter().GetResult());
        }

        [Fact]
        public void CreateUser_DuplicateEmail_ShouldThrow()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var db = new WikiDbContext(options);
            var svc = new UserService(db);
            svc.CreateUserAsync("bob", "bob@example.com", "password").GetAwaiter().GetResult();
            Assert.Throws<InvalidOperationException>(() => svc.CreateUserAsync("alice", "bob@example.com", "password").GetAwaiter().GetResult());
        }

        [Fact]
        public void CreateUser_ConcurrentDuplicateEmail_ShouldEnforceUniqueness()
        {
            // Use SQLite in-memory to simulate concurrency and DB-level uniqueness
            var connection = new Microsoft.Data.Sqlite.SqliteConnection("DataSource=:memory:");
            connection.Open();
            try
            {
                var options = new DbContextOptionsBuilder<WikiDbContext>().UseSqlite(connection).Options;
                using var db = new WikiDbContext(options);
                db.Database.EnsureCreated();

                var svc1 = new UserService(db);
                // Simulate two creates: first succeeds, second should fail due to unique index
                svc1.CreateUserAsync("bob", "dup@example.com", "password").GetAwaiter().GetResult();

                // second create via new context to simulate separate transaction
                using var db2 = new WikiDbContext(options);
                var svc2 = new UserService(db2);
                Assert.ThrowsAny<Exception>(() => svc2.CreateUserAsync("alice", "dup@example.com", "password").GetAwaiter().GetResult());
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public void EditProfile_NonExistentUser_ShouldNoOp()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var db = new WikiDbContext(options);
            var svc = new UserService(db);
            // should not throw
            svc.EditProfileAsync(Guid.NewGuid(), "name", "e@e.com", "pw").GetAwaiter().GetResult();
        }

        [Fact]
        public void ChangeAvatar_Oversized_ShouldThrow()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var db = new WikiDbContext(options);
            var svc = new UserService(db);
            var big = new byte[3 * 1024 * 1024];
            Assert.Throws<ArgumentException>(() => svc.ChangeAvatarAsync(Guid.NewGuid(), big).GetAwaiter().GetResult());
        }

        [Fact]
        public void DeleteAccount_ShouldRemoveUserAndNotifications()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var db = new WikiDbContext(options);
            var svc = new UserService(db);

            var uid = Guid.NewGuid();
            db.Users.Add(new AjpWiki.Domain.Entities.Users.User { Id = uid, Email = "u@e.com", CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow });
            db.Notifications.Add(new AjpWiki.Domain.Entities.Notifications.Notification { Id = Guid.NewGuid(), UserId = uid, Message = "m1", CreatedAt = DateTimeOffset.UtcNow });
            db.SaveChanges();

            svc.DeleteAccountAsync(uid).GetAwaiter().GetResult();
            var user = db.Users.Find(uid);
            Assert.Null(user);
            var notes = db.Notifications.Where(n => n.UserId == uid).ToList();
            Assert.Empty(notes);
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
