using System;
using System.Collections.Generic;
using AjpWiki.Domain.Entities.Articles;

namespace AjpWiki.Domain.Entities.Articles.Components
{
    /// <summary>
    /// Represents a lightweight grid layout container which can be used to arrange other component references.
    /// The grid stores cells with references (by component id) and optional layout hints like column spans.
    /// </summary>
    public class GridLayoutComponent : ArticleComponent
    {
        public class Cell
        {
            // Id of the component placed into this cell (must exist in the same version's Components collection)
            public Guid? ComponentId { get; set; }

            // Column span (1-based)
            public int ColSpan { get; set; } = 1;
            public int RowSpan { get; set; } = 1;
        }

        // Number of columns in the grid (for layout engines)
        public int Columns { get; set; } = 12;

        // Cells in row-major order
        public List<Cell> Cells { get; set; } = new List<Cell>();
    }
}
