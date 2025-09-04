using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AjpWiki.Application.Dto;

namespace AjpWiki.Application.Services
{
    public interface IWikiService
    {
        Task<WikiArticleDto?> GetAsync(Guid id);
        Task<IEnumerable<WikiArticleDto>> ListAsync();
    }
}
