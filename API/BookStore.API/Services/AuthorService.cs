using BookStore.API.Entities;
using BookStore.API.Helpers;
using BookStore.API.Models;
using BookStore.API.QueryParameters;
using BookStore.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Services
{
    public interface IAuthorService
    {
        Task<PagedResult<Author>> GetAuthors(AuthorQueryParameters authorQueryParameters);
        Task<Author> GetAuthorByBookIdAsync(int bookId);
    }

    public class AuthorService(IAuthorRepository authorRepository) : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository = authorRepository;

        public async Task<PagedResult<Author>> GetAuthors(AuthorQueryParameters authorQueryParameters)
        {
            ValidationHelper.ValidatePagination(authorQueryParameters.PageNumber, authorQueryParameters.PageSize);

            var authors = _authorRepository.GetAuthors(authorQueryParameters);

            var totalItemCount = await authors.CountAsync();
            var paginationMetaData = new PaginationMetaData(totalItemCount, authorQueryParameters.PageSize, authorQueryParameters.PageNumber);

            var result = await authors.OrderBy(a => a.Name)
                .Skip(authorQueryParameters.PageSize * (authorQueryParameters.PageNumber - 1))
                .Take(authorQueryParameters.PageSize)
                .ToListAsync();

            return new PagedResult<Author>(result, paginationMetaData);
        }

        public async Task<Author> GetAuthorByBookIdAsync(int bookId)
        {
            var result = await _authorRepository.GetAuthorByBookIdAsync(bookId);

            return result is null ? throw new KeyNotFoundException($"Author not found for book ID {bookId}") : result;
        }
    }
}
