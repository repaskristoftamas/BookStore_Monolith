using BookStore.API.Entities;
using BookStore.API.Helpers;
using BookStore.API.Models;
using BookStore.API.QueryParameters;
using BookStore.API.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookStore.API.Services
{
    public interface IGenreService
    {
        Task<PagedResult<Genre>> GetGenres(GenreQueryParameters genreQueryParameters);
    }

    public class GenreService(IGenreRepository genreRepository) : IGenreService
    {
        private readonly IGenreRepository _genreRepository = genreRepository;

        public async Task<PagedResult<Genre>> GetGenres(GenreQueryParameters genreQueryParameters)
        {
            ValidationHelper.ValidatePagination(genreQueryParameters.PageNumber, genreQueryParameters.PageSize);

            var genres = _genreRepository.GetGenres(genreQueryParameters);

            var totalItemCount = await genres.CountAsync();
            var paginationMetaData = new PaginationMetaData(totalItemCount, genreQueryParameters.PageSize, genreQueryParameters.PageNumber);

            var result = await genres.OrderBy(g => g.Name)
                .Skip(genreQueryParameters.PageSize * (genreQueryParameters.PageNumber - 1))
                .Take(genreQueryParameters.PageSize)
                .ToListAsync();

            return new PagedResult<Genre>(result, paginationMetaData);
        }
    }
}
