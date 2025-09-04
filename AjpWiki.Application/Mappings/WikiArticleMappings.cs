using AjpWiki.Application.Dto;
using AjpWiki.Domain.Entities;

namespace AjpWiki.Application.Mappings
{
    public static class WikiArticleMappings
    {
        public static WikiArticleDto ToDto(this WikiArticle e) => new(e.Id, e.Title);
    }
}
