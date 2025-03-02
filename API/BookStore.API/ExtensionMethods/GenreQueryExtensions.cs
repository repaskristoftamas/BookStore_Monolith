using BookStore.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.ExtensionMethods
{
    public static class GenreQueryExtensions
    {
        public static IQueryable<Genre> FilterByName(this IQueryable<Genre> genres, string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                return genres.Where(g => g.Name == name.Trim());

            return genres;
        }

        public static IQueryable<Genre> FilterByBook(this IQueryable<Genre> genres, string? title)
        {
            if (!string.IsNullOrWhiteSpace(title))
                return genres.Where(g => g.Books.Any(b => b.Title == title.Trim()));

            return genres;
        }

        public static IQueryable<Genre> SearchInDb(this IQueryable<Genre> genres, string? searchQuery)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
                return genres.Where(g => EF.Functions.Like(g.Name, $"%{searchQuery.Trim()}%"));

            return genres;
        }

        public static IEnumerable<Genre> SearchInMemory(this IEnumerable<Genre> genres, string? searchQuery)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
                return genres.Where(g => g.Name.Contains(searchQuery.Trim(), StringComparison.OrdinalIgnoreCase));

            return genres;
        }
    }
}
