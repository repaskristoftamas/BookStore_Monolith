using BookStore.API.DbContexts;
using BookStore.API.Entities;
using BookStore.API.ExtensionMethods;
using BookStore.API.QueryParameters;

namespace BookStore.API.Repositories
{
    public class AuthorRepository(BookStoreDbContext context) : IAuthorRepository
    {
        private readonly BookStoreDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public IQueryable<Author> GetAuthors(AuthorQueryParameters authorQueryParameters)
        {
            var authors = _context.Set<Author>().AsQueryable();

            return authors.FilterByName(authorQueryParameters.Query);
        }

        public async Task<Author?> GetAuthorByBookIdAsync(int bookId) => await _context.Set<Author>().FilterByBookIdAsync(bookId);
    }
}
