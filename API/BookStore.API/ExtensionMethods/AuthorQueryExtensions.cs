using BookStore.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.ExtensionMethods
{
    public static class AuthorQueryExtensions
    {
        public static IQueryable<Author> FilterByName(this IQueryable<Author> authors, string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                return authors.Where(a => a.Name == name.Trim());

            return authors;
        }

        public static IQueryable<Author> FilterByBook(this IQueryable<Author> authors, string? title)
        {
            if (!string.IsNullOrWhiteSpace(title))
                return authors.Where(a => a.Books.Any(b => b.Title == title.Trim()));

            return authors;
        }

        public static async Task<Author?> FilterByBookIdAsync(this IQueryable<Author> authors, int bookId)
            => await authors.FirstOrDefaultAsync(a => a.Books.Any(b => b.Id == bookId))
            ?? throw new ArgumentException("Invalid book ID. It must be greater than zero.");

        public static IQueryable<Author> SearchInDb(this IQueryable<Author> authors, string? searchQuery)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
                return authors.Where(a => EF.Functions.Like(a.Name, $"%{searchQuery.Trim()}%"));

            return authors;
        }

        public static IEnumerable<Author> SearchInMemory(this IEnumerable<Author> authors, string? searchQuery)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
                return authors.Where(a => a.Name.Contains(searchQuery.Trim(), StringComparison.OrdinalIgnoreCase));

            return authors;
        }
    }
}
