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
            svc.SendNotificationAsync(uid, "hello").GetAwaiter().GetResult();

            var messages = db.Notifications.Where(n => n.UserId == uid).Select(n => n.Message).ToList();
            Assert.Single(messages);
            Assert.Equal("hello", messages.First());
        }

        [Fact]
        public void GetNotifications_ShouldReturnAllUserNotifications()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new WikiDbContext(options);
            var uid = Guid.NewGuid();
            db.Notifications.Add(new AjpWiki.Domain.Entities.Notifications.Notification { Id = Guid.NewGuid(), UserId = uid, Message = "m1", CreatedAt = DateTimeOffset.UtcNow });
            db.Notifications.Add(new AjpWiki.Domain.Entities.Notifications.Notification { Id = Guid.NewGuid(), UserId = uid, Message = "m2", CreatedAt = DateTimeOffset.UtcNow });
            db.SaveChanges();

            var svc = new NotificationService(db);
            var list = svc.GetNotificationsAsync(uid).GetAwaiter().GetResult();
            Assert.Equal(2, list.Count());
        }
    }
}
