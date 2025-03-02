using BookStore.API.Entities;
using BookStore.API.QueryParameters;

namespace BookStore.API.Repositories
{
    public interface IGenreRepository
    {
        IQueryable<Genre> GetGenres(GenreQueryParameters genreQueryParameters);
    }
}
