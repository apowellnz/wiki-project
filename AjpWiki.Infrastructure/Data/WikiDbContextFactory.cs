using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AjpWiki.Infrastructure.Data;

public class WikiDbContextFactory : IDesignTimeDbContextFactory<WikiDbContext>
{
    public WikiDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<WikiDbContext>()
            .UseInMemoryDatabase("WikiInMemory")
            .Options;
        return new WikiDbContext(options);
    }
}