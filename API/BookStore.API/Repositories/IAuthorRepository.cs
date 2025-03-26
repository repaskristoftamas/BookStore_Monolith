using BookStore.API.Entities;
using BookStore.API.QueryParameters;

namespace BookStore.API.Repositories
{
    public interface IAuthorRepository
    {
        IQueryable<Author> GetAuthors(AuthorFilterOptions authorQueryParameters);
        Task<Author?> GetAuthorDetailsByIdAsync(int authorId);
        Task<Author> CreateAuthorAsync(Author author);
        Task<Author?> GetAuthorByNameAsync(string authorName);
    }
}
