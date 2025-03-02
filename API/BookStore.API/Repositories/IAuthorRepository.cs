using BookStore.API.Entities;
using BookStore.API.QueryParameters;

namespace BookStore.API.Repositories
{
    public interface IAuthorRepository
    {
        IQueryable<Author> GetAuthors(AuthorQueryParameters authorQueryParameters);
    }
}
