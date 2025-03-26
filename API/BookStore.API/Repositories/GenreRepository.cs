using BookStore.API.DbContexts;
using BookStore.API.Entities;
using BookStore.API.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Repositories
{
    public class GenreRepository(BookStoreDbContext context) : IGenreRepository
    {
        private readonly BookStoreDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public IQueryable<Genre> GetGenres(GenreFilterOptions genreQueryParameters)
        {
            var genres = _context.Set<Genre>().AsQueryable();

            return FilterByName(genres, genreQueryParameters.Query);
        }

        public async Task<Genre?> GetGenreDetailsByIdAsync(int genreId)
        {
            return await _context.Set<Genre>().FirstOrDefaultAsync(a => a.Id == genreId);
        }

        public async Task<Genre> CreateGenreAsync(Genre genre)
        {
            _context.Set<Genre>().Add(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task<Genre?> GetGenreByNameAsync(string genreName) =>
            await _context.Set<Genre>().FirstOrDefaultAsync(a => a.Name == genreName);

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
