using BookStore.API.Entities;
using BookStore.API.QueryParameters;

namespace BookStore.API.Repositories
{
    public interface IBookRepository
    {
        IQueryable<Book> GetBooks(BookQueryParameters bookQueryParameters);
    }
}
