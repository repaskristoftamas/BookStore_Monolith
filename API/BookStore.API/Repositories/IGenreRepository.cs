using BookStore.API.Entities;
using BookStore.API.QueryParameters;

namespace BookStore.API.Repositories
{
    public interface IGenreRepository
    {
        IQueryable<Genre> GetGenres(GenreFilterOptions genreQueryParameters);
        Task<Genre?> GetGenreDetailsByIdAsync(int genreId);
        Task<Genre> CreateGenreAsync(Genre genre);
        Task<Genre?> GetGenreByNameAsync(string genreName);
    }
}
