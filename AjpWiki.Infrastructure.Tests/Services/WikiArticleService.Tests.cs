using Xunit;
using System;
using Microsoft.EntityFrameworkCore;
using AjpWiki.Infrastructure.Data;
using AjpWiki.Infrastructure.Repositories;
using AjpWiki.Infrastructure.Services;
using AjpWiki.Domain.Entities.Articles;
using System.Linq;

namespace AjpWiki.Infrastructure.Tests.Services
{
    public class WikiArticleServiceTests
    {
        // User Story 3: Wiki Page Editing
        [Fact]
        public void EditArticle_ShouldUpdateArticleContent()
        {
            // Arrange
            // Act
            // Assert
        }

        // User Story 4: Versioning
        [Fact]
        public void EditArticle_ShouldCreateNewVersion()
        {
            // Arrange
            // Act
            // Assert
        }

        // User Story 5: Change History & Rollback
        [Fact]
        public void GetArticleHistory_ShouldReturnAllVersions()
        {
            // Arrange
            // Act
            // Assert
        }

        [Fact]
        public void RollbackArticle_ShouldRestorePreviousVersion()
        {
            // Arrange
            // Act
            // Assert
        }

        // User Story 6: Component Editing
        [Fact]
        public void AddComponent_ShouldAddComponentToArticle()
        {
            // Arrange
            // Act
            // Assert
        }

        // User Story 7: Discussion/Chat (placeholder, if implemented as service)

        // User Story 8: Change Review
        [Fact]
        public void ProposeChange_ShouldCreateChangeRequest()
        {
            // Arrange
            // Act
            // Assert
        }

        [Fact]
        public void ReviewChange_ShouldAcceptOrRejectChange()
        {
            // Arrange
            // Act
            // Assert
        }

        // User Story 9: Search & Navigation
        [Fact]
        public void SearchArticles_ShouldReturnMatchingArticles()
        {
            // Arrange
            // Act
            // Assert
        }

        // User Story 10: Article Creation
        [Fact]
        public void CreateArticle_ShouldAddNewArticle()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WikiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new WikiDbContext(options);
            var repo = new EfWikiArticleRepository(db);
            var service = new WikiArticleService(repo);

            var articleDto = new AjpWiki.Application.Dto.WikiArticleDto(Guid.Empty, "Test Article", null, null, false);
            var createdDto = service.CreateArticleAsync(articleDto).GetAwaiter().GetResult();

            var list = repo.ListAsync().GetAwaiter().GetResult();
            Assert.Single(list);
            Assert.Equal("Test Article", list.First().Title);
        }

        // User Story 15: Tagging/Categorization
        [Fact]
        public void GetArticlesByTag_ShouldReturnTaggedArticles()
        {
            // Arrange
            // Act
            // Assert
        }
    }
}
