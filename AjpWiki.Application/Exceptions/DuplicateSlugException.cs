using System;

namespace AjpWiki.Application.Exceptions
{
    public class DuplicateSlugException : Exception
    {
        public DuplicateSlugException(string? message) : base(message) { }
    }
}
