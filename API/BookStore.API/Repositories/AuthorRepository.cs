using BookStore.API.DbContexts;
using BookStore.API.Entities;
using BookStore.API.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Repositories
{
    public class AuthorRepository(BookStoreDbContext context) : IAuthorRepository
    {
        private readonly BookStoreDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public IQueryable<Author> GetAuthors(AuthorQueryParameters authorQueryParameters)
        {
            var authors = _context.Set<Author>().AsQueryable();

            return FilterByName(authors, authorQueryParameters.Query);
        }

        private IQueryable<Author> FilterByName(IQueryable<Author> authors, string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                return authors.Where(a => a.Name == name.Trim());

            return authors;
        }

        private IQueryable<Author> FilterByBook(IQueryable<Author> authors, string? title)
        {
            if (!string.IsNullOrWhiteSpace(title))
                return authors.Where(a => a.Books.Any(b => b.Title == title.Trim()));

            return authors;
        }

        private async Task<Author?> FilterByBookIdAsync(IQueryable<Author> authors, int bookId)
            => await authors.FirstOrDefaultAsync(a => a.Books.Any(b => b.Id == bookId))
            ?? throw new ArgumentException("Invalid book ID. It must be greater than zero.");

        private IQueryable<Author> SearchInDb(IQueryable<Author> authors, string? searchQuery)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
                return authors.Where(a => EF.Functions.Like(a.Name, $"%{searchQuery.Trim()}%"));

            return authors;
        }

        private IEnumerable<Author> SearchInMemory(IEnumerable<Author> authors, string? searchQuery)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
                return authors.Where(a => a.Name.Contains(searchQuery.Trim(), StringComparison.OrdinalIgnoreCase));

            return authors;
        }
    }
}
