using Xunit;
using System;
using Microsoft.EntityFrameworkCore;
using AjpWiki.Infrastructure.Data;
using AjpWiki.Infrastructure.Repositories;
using AjpWiki.Infrastructure.Services;
using AjpWiki.Application.Utils;
using AjpWiki.Application.Exceptions;
using System.Linq;

namespace AjpWiki.Infrastructure.Tests.Services
{
    public class WikiArticleServiceTests
    {
        // Note: placeholder tests removed to keep suite focused; implemented tests follow.

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

            var articleDto = new AjpWiki.Application.Dto.WikiArticleDto(Guid.Empty, "Test Article", null, null, null, false);
            var createdDto = service.CreateArticleAsync(articleDto).GetAwaiter().GetResult();

            var list = repo.ListAsync().GetAwaiter().GetResult();
            Assert.Single(list);
            Assert.Equal("Test Article", list.First().Title);
        }

        [Fact]
        public void CreateArticle_WithDuplicateSlug_ShouldThrow()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new WikiDbContext(options);
            var repo = new EfWikiArticleRepository(db);
            var service = new WikiArticleService(repo);

            var slug = "test-article";
            // Create first article using the repository so we can set the Slug on the domain entity
            var a1 = new AjpWiki.Domain.Entities.Articles.WikiArticle { Title = "Test Article 1", Slug = slug };
            var created1 = repo.CreateArticleAsync(a1).GetAwaiter().GetResult();

            var a2 = new AjpWiki.Domain.Entities.Articles.WikiArticle { Title = "Test Article 2", Slug = slug };
            Assert.Throws<InvalidOperationException>(() => repo.CreateArticleAsync(a2).GetAwaiter().GetResult());
        }

        [Fact]
        public void CreateArticle_GeneratesSlug_WhenMissing()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new WikiDbContext(options);
            var repo = new EfWikiArticleRepository(db);
            var service = new WikiArticleService(repo);

            var title = "My Test Article";
            var articleDto = new AjpWiki.Application.Dto.WikiArticleDto(Guid.Empty, title, null, null, null, false);
            var created = service.CreateArticleAsync(articleDto).GetAwaiter().GetResult();

            Assert.False(string.IsNullOrWhiteSpace(created.Slug));
            Assert.Equal(SlugHelper.Generate(title), created.Slug);
        }

        [Fact]
        public void CreateArticle_ServiceDuplicateSlug_ThrowsDuplicateSlugException()
        {
            var options = new DbContextOptionsBuilder<WikiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new WikiDbContext(options);
            var repo = new EfWikiArticleRepository(db);
            var service = new WikiArticleService(repo);

            var slug = "test-article";
            // Create first article using the repository so we can set the Slug on the domain entity
            var a1 = new AjpWiki.Domain.Entities.Articles.WikiArticle { Title = "Test Article 1", Slug = slug };
            var created1 = repo.CreateArticleAsync(a1).GetAwaiter().GetResult();

            var dto = new AjpWiki.Application.Dto.WikiArticleDto(Guid.Empty, "Test Article", null, null, null, false);
            Assert.Throws<DuplicateSlugException>(() => service.CreateArticleAsync(dto).GetAwaiter().GetResult());
        }

        // User Story 15: Tagging/Categorization
    }
}
