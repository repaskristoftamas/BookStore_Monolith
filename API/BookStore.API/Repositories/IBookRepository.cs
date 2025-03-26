using BookStore.API.Entities;
using BookStore.API.QueryParameters;

namespace BookStore.API.Repositories
{
    public interface IBookRepository
    {
        IQueryable<Book> GetBooks(BookFilterOptions filterOptions);
        Task<Book?> GetBookDetailsByIdAsync(int bookId);
        Task<Book> CreateBookAsync(Book book);
    }
}
