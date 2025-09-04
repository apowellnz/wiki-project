using System;
using System.Collections.Generic;

namespace AjpWiki.Domain.Entities.Articles
{
    /// <summary>
    /// Base class for components that make up an article version. Keep it simple and serializable.
    /// Concrete components can add typed properties; additional custom components can be introduced
    /// by providing a derived type and mapping in the Application layer.
    /// </summary>
    public abstract class ArticleComponent
    {
        public Guid Id { get; set; }

        // Position within the version's component list. Application layer should maintain ordering.
        public int Order { get; set; }

        public ComponentType Type { get; set; }
    }

    public enum ComponentType
    {
        Title = 0,
        RichText = 1,
        Image = 2,
        Table = 3,
        List = 4,
        // Allows future custom widgets to be added without changing existing consumers
        Custom = 100
    }
}
