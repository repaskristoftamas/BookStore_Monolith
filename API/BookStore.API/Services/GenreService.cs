using AutoMapper;
using BookStore.API.Entities;
using BookStore.API.Models;
using BookStore.API.QueryParameters;
using BookStore.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Services
{
    public interface IGenreService
    {
        Task<PagedResult<GenreDto>> GetGenres(GenreFilterOptions genreQueryParameters);
        Task<GenreDto?> GetGenreDetailsById(int genreId);
        Task<GenreDto> CreateGenreAsync(GenreForCreationDto genreDto);
    }

    public class GenreService(IGenreRepository genreRepository, IMapper mapper) : IGenreService
    {
        private readonly IGenreRepository _genreRepository = genreRepository;
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<PagedResult<GenreDto>> GetGenres(GenreFilterOptions genreQueryParameters)
        {
            //ValidationHelper.ValidatePagination(genreQueryParameters.PageNumber, genreQueryParameters.PageSize);

            var genres = _genreRepository.GetGenres(genreQueryParameters);

            var totalItemCount = await genres.CountAsync();
            //var paginationMetaData = new PaginationMetaData(totalItemCount, genreQueryParameters.PageSize, genreQueryParameters.PageNumber);

            var result = await genres.OrderBy(g => g.Name).ToListAsync();
                //.Skip(genreQueryParameters.PageSize * (genreQueryParameters.PageNumber - 1))
                //.Take(genreQueryParameters.PageSize)
                //.ToListAsync();

            var pagedResult = new PagedResult<Genre>(result, null/*paginationMetaData*/);

            return _mapper.Map<PagedResult<GenreDto>>(pagedResult);

            //return new PagedResult<Author>(result, paginationMetaData);
        }

        public async Task<GenreDto?> GetGenreDetailsById(int genreId)
        {
            if (genreId < 1) throw new InvalidOperationException("Genre id is invalid.");

            var result = await _genreRepository.GetGenreDetailsByIdAsync(genreId);

            return result is null ? throw new KeyNotFoundException($"Genre not found for genre ID {genreId}.") : _mapper.Map<GenreDto>(result);
        }

        public async Task<GenreDto> CreateGenreAsync(GenreForCreationDto genreDto)
        {
            var genre = _mapper.Map<Genre>(genreDto);

            var result = await GetOrCreateGenreAsync(genre);

            return _mapper.Map<GenreDto>(result);
        }

        private async Task<Genre> GetOrCreateGenreAsync(Genre genre)
        {
            var existingGenre = await _genreRepository.GetGenreByNameAsync(genre.Name);

            if (existingGenre is not null) return existingGenre;

            var newGenre = new Genre { Name = genre.Name };
            await _genreRepository.CreateGenreAsync(newGenre);

            return newGenre;
        }
    }
}
