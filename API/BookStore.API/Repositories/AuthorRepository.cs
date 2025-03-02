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
            var (name, bookTitle, searchQuery) = authorQueryParameters;

            var authors = _context.Set<Author>().AsQueryable();

            return authors
                .FilterByName(name)
                .FilterByBook(bookTitle)
                .SearchInDb(searchQuery);
        }
    }
}
