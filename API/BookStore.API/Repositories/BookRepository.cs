using BookStore.API.DbContexts;
using BookStore.API.Entities;
using BookStore.API.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Repositories
{
    public class BookRepository(BookStoreDbContext context) : IBookRepository
    {
        private readonly BookStoreDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public IQueryable<Book> GetBooks(FilterOptions filterOptions)
        {
            var books = _context.Set<Book>().AsQueryable();

            if (filterOptions.IncludeAuthor) books = books.Include(b => b.Author);
            if (filterOptions.IncludeGenre) books = books.Include(b => b.Genre);

            return filterOptions.FilterBy switch
            {
                "title" => FilterByTitle(books, filterOptions.Query),
                "author" => FilterByAuthor(books, filterOptions.Query),
                "genre" => FilterByGenre(books, filterOptions.Query),
                _ => books
            };
        }

        public async Task<Book?> GetBookDetailsByIdAsync(int bookId)
        {
            return await _context.Set<Book>()
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(b => b.Id == bookId);
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            _context.Set<Book>().Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Author?> GetAuthorByNameAsync(string authorName) =>
            await _context.Set<Author>().FirstOrDefaultAsync(a => a.Name == authorName);

        public async Task<Genre?> GetGenreByNameAsync(string genreName) =>
            await _context.Set<Genre>().FirstOrDefaultAsync(a => a.Name == genreName);

        public async Task AddAuthorAsync(Author author)
        {
            _context.Set<Author>().Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task AddGenreAsync(Genre genre)
        {
            _context.Set<Genre>().Add(genre);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Book> FilterByTitle(IQueryable<Book> books, string? title)
        {
            if (!string.IsNullOrWhiteSpace(title))
                return books.Where(b => b.Title == title.Trim());

            return books;
        }

        private IQueryable<Book> FilterByAuthor(IQueryable<Book> books, string? author)
        {
            if (!string.IsNullOrWhiteSpace(author))
                return books.Where(b => b.Author!.Name == author.Trim());

            return books;
        }

        private IQueryable<Book> FilterByGenre(IQueryable<Book> books, string? genre)
        {
            if (!string.IsNullOrWhiteSpace(genre))
                return books.Where(b => b.Genre!.Name == genre.Trim());

            return books;
        }
    }
}
