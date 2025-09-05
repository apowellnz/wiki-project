using AjpWiki.Domain.Entities.Articles;
using Microsoft.EntityFrameworkCore;

namespace AjpWiki.Infrastructure.Data;

public class WikiDbContext : DbContext
{
    public DbSet<WikiArticle> WikiArticles { get; set; } = null!;
    public DbSet<WikiArticleVersion> WikiArticleVersions { get; set; } = null!;
    public DbSet<AjpWiki.Domain.Entities.Users.User> Users { get; set; } = null!;
    public DbSet<AjpWiki.Domain.Entities.Notifications.Notification> Notifications { get; set; } = null!;

    public WikiDbContext(DbContextOptions<WikiDbContext> options) : base(options) { }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WikiArticle>().HasKey(e => e.Id);
        modelBuilder.Entity<WikiArticle>()
            .Property(e => e.Title).HasMaxLength(200);

        modelBuilder.Entity<WikiArticle>()
            .Property(e => e.Slug)
            .IsRequired(false)
            .HasMaxLength(2048); 

         modelBuilder.Entity<WikiArticle>()
            .HasIndex(e => e.Slug)
            .IsUnique();

        modelBuilder.Entity<WikiArticle>()
            .HasIndex(e => e.Title);

    modelBuilder.Entity<WikiArticleVersion>().HasKey(v => v.Id);

    modelBuilder.Entity<AjpWiki.Domain.Entities.Users.User>().HasKey(u => u.Id);
    // Make Email unique at the DB level to support uniqueness guarantees in integration tests.
    modelBuilder.Entity<AjpWiki.Domain.Entities.Users.User>().HasIndex(u => u.Email).IsUnique();

    modelBuilder.Entity<AjpWiki.Domain.Entities.Notifications.Notification>().HasKey(n => n.Id);
    }
}
