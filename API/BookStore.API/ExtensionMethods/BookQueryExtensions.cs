using BookStore.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.ExtensionMethods
{
    public static class BookQueryExtensions
    {
        public static IQueryable<Book> FilterByTitle(this IQueryable<Book> books, string? title)
        {
            if (!string.IsNullOrWhiteSpace(title))
                return books.Where(b => b.Title == title.Trim());

            return books;
        }

        public static IQueryable<Book> FilterByAuthor(this IQueryable<Book> books, string? author)
        {
            if (!string.IsNullOrWhiteSpace(author))
                return books.Where(b => b.Author!.Name == author.Trim());

            return books;
        }

        public static IQueryable<Book> FilterByGenre(this IQueryable<Book> books, string? genre)
        {
            if (!string.IsNullOrWhiteSpace(genre))
                return books.Where(b => b.Genre!.Name == genre.Trim());

            return books;
        }

        public static IQueryable<Book> SearchInDb(this IQueryable<Book> books, string? searchQuery)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
                return books.Where(b =>
                    EF.Functions.Like(b.Title, $"%{searchQuery.Trim()}%") ||
                    EF.Functions.Like(b.Author!.Name, $"%{searchQuery.Trim()}%") ||
                    EF.Functions.Like(b.Genre!.Name, $"%{searchQuery.Trim()}%")
                );

            return books;
        }

        public static IEnumerable<Book> SearchInMemory(this IEnumerable<Book> books, string? searchQuery)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
                return books.Where(b =>
                    b.Title.Contains(searchQuery.Trim(), StringComparison.OrdinalIgnoreCase) ||
                    b.Author!.Name.Contains(searchQuery.Trim(), StringComparison.OrdinalIgnoreCase) ||
                    b.Genre!.Name.Contains(searchQuery.Trim(), StringComparison.OrdinalIgnoreCase)
                );

            return books;
        }
    }
}
