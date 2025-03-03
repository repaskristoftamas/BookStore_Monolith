using BookStore.API.Entities;
using BookStore.API.QueryParameters;

namespace BookStore.API.Repositories
{
    public interface IBookRepository
    {
        IQueryable<Book> GetBooks(BookQueryParameters bookQueryParameters);
        Task<Book?> GetBookDetailsByIdAsync(int bookId);
        Task<Book> CreateBookAsync(Book book);
        Task<Author?> GetAuthorByNameAsync(string authorName);
        Task<Genre?> GetGenreByNameAsync(string genreName);
        Task AddAuthorAsync(Author author);
        Task AddGenreAsync(Genre genre);
    }
}
