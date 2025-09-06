using Xunit;
using System;
using AjpWiki.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AjpWiki.Infrastructure.Services;
using System.Linq;

namespace AjpWiki.Infrastructure.Tests.Services
{
    public class NotificationServiceTests
    {
        // User Story 13: Notifications
        [Fact]
        public void SendNotification_ShouldDeliverMessageToUser()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new WikiDbContext(options);
            var svc = new NotificationService(db);

            var uid = Guid.NewGuid();
            // create the user so send succeeds
            db.Users.Add(new AjpWiki.Domain.Entities.Users.User { Id = uid, Email = "u@example.com", CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow });
            db.SaveChanges();

            var created = svc.SendNotificationAsync(uid, "hello").GetAwaiter().GetResult();

            var messages = db.Notifications.Where(n => n.UserId == uid).Select(n => n.Message).ToList();
            Assert.Single(messages);
            Assert.Equal("hello", messages.First());
            Assert.Equal(uid, created.UserId);
            Assert.Equal("hello", created.Message);
            Assert.False(created.IsRead);
        }

        [Fact]
        public void GetNotifications_ShouldReturnAllUserNotifications()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new WikiDbContext(options);
            var uid = Guid.NewGuid();
            db.Users.Add(new AjpWiki.Domain.Entities.Users.User { Id = uid, Email = "u@example.com", CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow });
            db.Notifications.Add(new AjpWiki.Domain.Entities.Notifications.Notification { Id = Guid.NewGuid(), UserId = uid, Message = "m1", CreatedAt = DateTimeOffset.UtcNow });
            db.Notifications.Add(new AjpWiki.Domain.Entities.Notifications.Notification { Id = Guid.NewGuid(), UserId = uid, Message = "m2", CreatedAt = DateTimeOffset.UtcNow });
            db.SaveChanges();

            var svc = new NotificationService(db);
            var list = svc.GetNotificationsAsync(uid).GetAwaiter().GetResult();
            Assert.Equal(2, list.Count());
        }

        [Fact]
        public void SendNotification_ToNonExistentUser_ShouldThrow()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new WikiDbContext(options);
            var svc = new NotificationService(db);

            var uid = Guid.NewGuid();
            Assert.Throws<InvalidOperationException>(() => svc.SendNotificationAsync(uid, "hello").GetAwaiter().GetResult());
        }

        [Fact]
        public void GetNotifications_Pagination_ShouldReturnCorrectPages()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new WikiDbContext(options);
            var uid = Guid.NewGuid();
            db.Users.Add(new AjpWiki.Domain.Entities.Users.User { Id = uid, Email = "u@example.com", CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow });
            for (int i = 1; i <= 25; i++)
            {
                db.Notifications.Add(new AjpWiki.Domain.Entities.Notifications.Notification { Id = Guid.NewGuid(), UserId = uid, Message = $"m{i}", CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-i) });
            }
            db.SaveChanges();

            var svc = new NotificationService(db);
            var page1Dto = svc.GetNotificationsAsync(uid, 1, 10).GetAwaiter().GetResult();
            var page3Dto = svc.GetNotificationsAsync(uid, 3, 10).GetAwaiter().GetResult();
            var page1 = page1Dto.Items.ToList();
            var page3 = page3Dto.Items.ToList();

            Assert.Equal(10, page1.Count);
            Assert.Equal(5, page3.Count);
            Assert.Equal(25, page1Dto.TotalCount);
            Assert.Equal(1, page1Dto.Page);
            Assert.Equal(10, page1Dto.PageSize);
            // Ensure ordering: newest first (m1 was newest because -1 minute)
            Assert.Equal("m1", page1.First().Message);
        }

        [Fact]
        public void MarkAsRead_Idempotent_And_MissingIsNoOp()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new WikiDbContext(options);
            var uid = Guid.NewGuid();
            db.Users.Add(new AjpWiki.Domain.Entities.Users.User { Id = uid, Email = "u@example.com", CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow });
            var n = new AjpWiki.Domain.Entities.Notifications.Notification { Id = Guid.NewGuid(), UserId = uid, Message = "m1", CreatedAt = DateTimeOffset.UtcNow, IsRead = false };
            db.Notifications.Add(n);
            db.SaveChanges();

            var svc = new NotificationService(db);
            // mark first time
            svc.MarkAsReadAsync(n.Id).GetAwaiter().GetResult();
            var persisted = db.Notifications.Find(n.Id);
            Assert.True(persisted.IsRead);

            // mark again should be no-op and not throw
            svc.MarkAsReadAsync(n.Id).GetAwaiter().GetResult();

            // missing id should be no-op
            svc.MarkAsReadAsync(Guid.NewGuid()).GetAwaiter().GetResult();
        }

        [Fact]
        public void GetUnreadCount_And_MarkAllAsRead_Works()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new WikiDbContext(options);
            var uid = Guid.NewGuid();
            db.Users.Add(new AjpWiki.Domain.Entities.Users.User { Id = uid, Email = "u@example.com", CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow });

            db.Notifications.Add(new AjpWiki.Domain.Entities.Notifications.Notification { Id = Guid.NewGuid(), UserId = uid, Message = "m1", CreatedAt = DateTimeOffset.UtcNow, IsRead = false });
            db.Notifications.Add(new AjpWiki.Domain.Entities.Notifications.Notification { Id = Guid.NewGuid(), UserId = uid, Message = "m2", CreatedAt = DateTimeOffset.UtcNow, IsRead = false });
            db.Notifications.Add(new AjpWiki.Domain.Entities.Notifications.Notification { Id = Guid.NewGuid(), UserId = uid, Message = "m3", CreatedAt = DateTimeOffset.UtcNow, IsRead = true });
            db.SaveChanges();

            var svc = new NotificationService(db);
            var unreadBefore = svc.GetUnreadCountAsync(uid).GetAwaiter().GetResult();
            Assert.Equal(2, unreadBefore);

            svc.MarkAllAsReadAsync(uid).GetAwaiter().GetResult();
            var unreadAfter = svc.GetUnreadCountAsync(uid).GetAwaiter().GetResult();
            Assert.Equal(0, unreadAfter);
        }
    }
}
