using AjpWiki.Domain.Entities.Articles;
using Microsoft.EntityFrameworkCore;

namespace AjpWiki.Infrastructure.Data;

public class WikiDbContext : DbContext
{
    public DbSet<WikiArticle> WikiArticles { get; set; } = null!;

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
    }
}
