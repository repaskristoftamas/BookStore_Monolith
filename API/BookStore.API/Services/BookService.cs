using BookStore.API.Entities;
using BookStore.API.Helpers;
using BookStore.API.Models;
using BookStore.API.QueryParameters;
using BookStore.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Services
{
    public interface IBookService
    {
        Task<PagedResult<Book>> GetBooks(BookQueryParameters bookQueryParameters);
    }

    public class BookService(IBookRepository bookRepository) : IBookService
    {
        private readonly IBookRepository _bookRepository = bookRepository;

        public async Task<PagedResult<Book>> GetBooks(BookQueryParameters bookQueryParameters)
        {
            ValidationHelper.ValidatePagination(bookQueryParameters.PageNumber, bookQueryParameters.PageSize);

            var books = _bookRepository.GetBooks(bookQueryParameters);

            var totalItemCount = await books.CountAsync();
            var paginationMetaData = new PaginationMetaData(totalItemCount, bookQueryParameters.PageSize, bookQueryParameters.PageNumber);

            var result = await books.OrderBy(b => b.Title)
                .Skip(bookQueryParameters.PageSize * (bookQueryParameters.PageNumber - 1))
                .Take(bookQueryParameters.PageSize)
                .ToListAsync();

            return new PagedResult<Book>(result, paginationMetaData);
        }
    }
}
