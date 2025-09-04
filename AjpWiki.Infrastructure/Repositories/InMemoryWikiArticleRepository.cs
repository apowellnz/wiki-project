using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AjpWiki.Domain.Entities;
using AjpWiki.Domain.Repositories;

namespace AjpWiki.Infrastructure.Repositories
{
    public class InMemoryWikiArticleRepository : IWikiArticleRepository
    {
        private readonly List<WikiArticle> _store = new()
        {
            new WikiArticle { Id = Guid.NewGuid(), Title = "Welcome" }
        };

        public Task<WikiArticle?> GetByIdAsync(Guid id) => Task.FromResult(_store.FirstOrDefault(x => x.Id == id));

        public Task<IEnumerable<WikiArticle>> ListAsync() => Task.FromResult<IEnumerable<WikiArticle>>(_store);
    }
}
