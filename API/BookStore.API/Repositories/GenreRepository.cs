using BookStore.API.DbContexts;
using BookStore.API.Entities;
using BookStore.API.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Repositories
{
    public class GenreRepository(BookStoreDbContext context) : IGenreRepository
    {
        private readonly BookStoreDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public IQueryable<Genre> GetGenres(GenreQueryParameters genreQueryParameters)
        {
            var genres = _context.Set<Genre>().AsQueryable();

            return FilterByName(genres, genreQueryParameters.Query);
        }

        private IQueryable<Genre> FilterByName(IQueryable<Genre> genres, string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                return genres.Where(g => g.Name == name.Trim());

            return genres;
        }

        private IQueryable<Genre> FilterByBook(IQueryable<Genre> genres, string? title)
        {
            if (!string.IsNullOrWhiteSpace(title))
                return genres.Where(g => g.Books.Any(b => b.Title == title.Trim()));

            return genres;
        }

        private IQueryable<Genre> SearchInDb(IQueryable<Genre> genres, string? searchQuery)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
                return genres.Where(g => EF.Functions.Like(g.Name, $"%{searchQuery.Trim()}%"));

            return genres;
        }

        private IEnumerable<Genre> SearchInMemory(IEnumerable<Genre> genres, string? searchQuery)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
                return genres.Where(g => g.Name.Contains(searchQuery.Trim(), StringComparison.OrdinalIgnoreCase));

            return genres;
        }
    }
}
