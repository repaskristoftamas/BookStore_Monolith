using BookStore.API.DbContexts;
using BookStore.API.Entities;
using BookStore.API.ExtensionMethods;
using BookStore.API.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Repositories
{
    public class BookRepository(BookStoreDbContext context) : IBookRepository
    {
        private readonly BookStoreDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public IQueryable<Book> GetBooks(BookQueryParameters bookQueryParameters)
        {
            var (title, author, genre, includeAuthor, includeGenre, searchQuery) = bookQueryParameters;

            var books = _context.Set<Book>().AsQueryable();

            if (includeAuthor) books = books.Include(b => b.Author);
            if (includeGenre) books = books.Include(b => b.Genre);

            return books
                .FilterByTitle(title)
                .FilterByAuthor(author)
                .FilterByGenre(genre)
                .SearchInDb(searchQuery);
        }
    }
}
