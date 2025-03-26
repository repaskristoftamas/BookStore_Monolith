using AutoMapper;
using BookStore.API.Entities;
using BookStore.API.Models;
using BookStore.API.QueryParameters;
using BookStore.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Services
{
    public interface IAuthorService
    {
        Task<PagedResult<AuthorDto>> GetAuthors(AuthorFilterOptions authorQueryParameters);
        Task<AuthorDto?> GetAuthorDetailsById(int authorId);
        Task<AuthorDto> CreateAuthorAsync(AuthorForCreationDto authorDto);
    }

    public class AuthorService(IAuthorRepository authorRepository, IMapper mapper) : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<PagedResult<AuthorDto>> GetAuthors(AuthorFilterOptions authorQueryParameters)
        {
            //ValidationHelper.ValidatePagination(authorQueryParameters.PageNumber, authorQueryParameters.PageSize);

            var authors = _authorRepository.GetAuthors(authorQueryParameters);

            var totalItemCount = await authors.CountAsync();
            //var paginationMetaData = new PaginationMetaData(totalItemCount, authorQueryParameters.PageSize, authorQueryParameters.PageNumber);

            var result = await authors.OrderBy(a => a.Name).ToListAsync();
                //.Skip(authorQueryParameters.PageSize * (authorQueryParameters.PageNumber - 1))
                //.Take(authorQueryParameters.PageSize)
                //.ToListAsync();

            var pagedResult = new PagedResult<Author>(result, null/*paginationMetaData*/);

            return _mapper.Map<PagedResult<AuthorDto>>(pagedResult);

            //return new PagedResult<Author>(result, paginationMetaData);
        }

        public async Task<AuthorDto?> GetAuthorDetailsById(int authorId)
        {
            if (authorId < 1) throw new InvalidOperationException("Author id is invalid.");

            var result = await _authorRepository.GetAuthorDetailsByIdAsync(authorId);

            return result is null ? throw new KeyNotFoundException($"Author not found for author ID {authorId}.") : _mapper.Map<AuthorDto>(result);
        }

        public async Task<AuthorDto> CreateAuthorAsync(AuthorForCreationDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);

            var result = await GetOrCreateAuthorAsync(author);

            return _mapper.Map<AuthorDto>(result);
        }

        private async Task<Author> GetOrCreateAuthorAsync(Author author)
        {
            var existingAuthor = await _authorRepository.GetAuthorByNameAsync(author.Name);

            if (existingAuthor is not null) return existingAuthor;

            var newAuthor = new Author { Name = author.Name };
            await _authorRepository.CreateAuthorAsync(newAuthor);

            return newAuthor;
        }
    }
}
