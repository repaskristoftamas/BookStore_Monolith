using AutoMapper;
using BookStore.API.Entities;
using BookStore.API.Models;
using BookStore.API.QueryParameters;
using BookStore.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Services
{
    public interface IBookService
    {
        Task<PagedResult<BookDto>> GetBooksAsync(BookFilterOptions filterOptions);
        Task<BookDto?> GetBookDetailsByIdAsync(int bookId);
        Task<BookDto> CreateBookAsync(BookForCreationDto bookDto);
    }

    public class BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IGenreRepository genreRepository, IMapper mapper) : IBookService
    {
        private readonly IBookRepository _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        private readonly IAuthorRepository _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
        private readonly IGenreRepository _genreRepository = genreRepository ?? throw new ArgumentNullException(nameof(genreRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<PagedResult<BookDto>> GetBooksAsync(BookFilterOptions filterOptions)
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

            book.Author = book.Author is null ? throw new InvalidOperationException("Author is required.") : await GetOrCreateAuthorAsync(book.Author);

            if (book.Genre is not null)
                await GetOrCreateGenreAsync(book.Genre);

            var result = await _bookRepository.CreateBookAsync(book);

            return _mapper.Map<BookDto>(result);
        }

        private async Task<Author> GetOrCreateAuthorAsync(Author author)
        {
            var existingAuthor = await _authorRepository.GetAuthorByNameAsync(author.Name);

            if (existingAuthor is not null) return existingAuthor;

            var newAuthor = new Author { Name = author.Name };
            await _authorRepository.CreateAuthorAsync(newAuthor);

            return newAuthor;
        }

        private async Task<Genre> GetOrCreateGenreAsync(Genre genre)
        {
            var existingGenre = await _genreRepository.GetGenreByNameAsync(genre.Name);

            if (existingGenre is not null) return existingGenre;

            var newGenre = new Genre { Name = genre.Name };
            await _genreRepository.CreateGenreAsync(genre);

            return newGenre;
        }
    }
}
