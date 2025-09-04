using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AjpWiki.Domain.Repositories
{
    public interface IWikiArticleRepository
    {
        Task<WikiArticle?> GetByIdAsync(Guid id);
        Task<IEnumerable<WikiArticle>> ListAsync();
    }
}
