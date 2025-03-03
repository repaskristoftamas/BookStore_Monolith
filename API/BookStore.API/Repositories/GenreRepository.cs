using BookStore.API.DbContexts;
using BookStore.API.Entities;
using BookStore.API.ExtensionMethods;
using BookStore.API.QueryParameters;

namespace BookStore.API.Repositories
{
    public class GenreRepository(BookStoreDbContext context) : IGenreRepository
    {
        private readonly BookStoreDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public IQueryable<Genre> GetGenres(GenreQueryParameters genreQueryParameters)
        {
            var genres = _context.Set<Genre>().AsQueryable();

            return genres.FilterByName(genreQueryParameters.Query);
        }
    }
}
