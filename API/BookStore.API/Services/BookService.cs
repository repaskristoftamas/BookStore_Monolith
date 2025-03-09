using AutoMapper;
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
        Task<PagedResult<BookDto>> GetBooksAsync(FilterOptions filterOptions);
        Task<BookDto?> GetBookDetailsByIdAsync(int bookId);
        Task<BookDto> CreateBookAsync(BookForCreationDto bookDto);
    }

    public class BookService(IBookRepository bookRepository, IMapper mapper) : IBookService
    {
        private readonly IBookRepository _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<PagedResult<BookDto>> GetBooksAsync(FilterOptions filterOptions)
        {
            //ValidationHelper.ValidatePagination(filterOptions.PageNumber, filterOptions.PageSize);

            var books = _bookRepository.GetBooks(filterOptions);

            var totalItemCount = await books.CountAsync();
            //var paginationMetaData = new PaginationMetaData(totalItemCount, filterOptions.PageSize, filterOptions.PageNumber);

            var result = await books.OrderBy(b => b.Title).ToListAsync();
                //.Skip(filterOptions.PageSize * (filterOptions.PageNumber - 1))
                //.Take(filterOptions.PageSize)
                //.ToListAsync();

            var pagedResult = new PagedResult<Book>(result, null/*paginationMetaData*/);

            return _mapper.Map<PagedResult<BookDto>>(pagedResult);
        }

        public async Task<BookDto?> GetBookDetailsByIdAsync(int bookId)
        {
            if (bookId < 1) throw new InvalidOperationException("Book id is invalid.");

            var result = await _bookRepository.GetBookDetailsByIdAsync(bookId);

            return result is null ? throw new KeyNotFoundException($"Book not found for book ID {bookId}.") : _mapper.Map<BookDto>(result);
        }

        public async Task<BookDto> CreateBookAsync(BookForCreationDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);

            if (book.Author is null) throw new InvalidOperationException("Author is required.");

            var existingAuthor = await _bookRepository.GetAuthorByNameAsync(book.Author.Name);

            if (existingAuthor is null)
            {
                existingAuthor = new Author { Name = book.Author.Name };
                await _bookRepository.AddAuthorAsync(existingAuthor);
            }

            book.Author = existingAuthor;

            if (book.Genre is not null)
            {
                var existingGenre = await _bookRepository.GetGenreByNameAsync(book.Genre.Name);

                if (existingGenre is null)
                {
                    existingGenre = new Genre { Name = book.Genre.Name };
                    await _bookRepository.AddGenreAsync(existingGenre);
                }

                book.Genre = existingGenre;
            }

            var result = await _bookRepository.CreateBookAsync(book);

            return _mapper.Map<BookDto>(result);
        }
    }
}
