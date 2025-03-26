using BookStore.API.DbContexts;
using BookStore.API.Entities;
using BookStore.API.QueryParameters;
using BookStore.API.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Tests.RepositoryIntegrationTests
{
    public class BookRepositoryIntegrationTests
    {
        private readonly BookStoreDbContext _context;
        private readonly BookRepository _repository;

        public BookRepositoryIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<BookStoreDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            _context = new BookStoreDbContext(options);
            SeedInMemoryDatabase();
            _repository = new BookRepository(_context);
        }

        private void SeedInMemoryDatabase()
        {
            ResetDatabase();

            var books = new List<Book>
            {
                new() { Id = 1, Title = "Harry Potter and the Philosopher's Stone", Author = new Author { Name = "J.K. Rowling" }, Genre = new Genre { Name = "Fantasy" } },
                new() { Id = 2, Title = "Harry Potter and the Chamber of Secrets", Author = new Author { Name = "J.K. Rowling" }, Genre = new Genre { Name = "Fantasy" } },
                new() { Id = 3, Title = "Harry Potter and the Prisoner of Azkaban", Author = new Author { Name = "J.K. Rowling" }, Genre = new Genre { Name = "Fantasy" } },
                new() { Id = 4, Title = "Foundation", Author = new Author { Name = "Isaac Asimov"}, Genre = new Genre {Name = "Science Fiction" } },
                new() { Id = 5, Title = "Murder on the Orient Express", Author = new Author { Name = "Agatha Christie"}, Genre = new Genre {Name = "Mystery" } },
                new() { Id = 6, Title = "The Hobbit", Author = new Author { Name = "J.R.R. Tolkien"}, Genre = new Genre {Name = "Historical Fiction" } },
                new() { Id = 7, Title = "The Shining", Author = new Author { Name = "Stephen King"}, Genre = new Genre {Name = "Horror" } },
                new() { Id = 8, Title = "1984", Author = new Author { Name = "George Orwell"}, Genre = new Genre {Name = "Science Fiction" } },
                new() { Id = 9, Title = "Frankenstein", Author = new Author { Name = "Mary Shelley"}, Genre = new Genre {Name = "Horror" } },
                new() { Id = 10, Title = "The Da Vinci Code", Author = new Author { Name = "Dan Brown"}, Genre = new Genre {Name = "Mystery" } },
                new() { Id = 11, Title = "Európa elrablása", Author = new Author { Name = "Puzsér Róbert"}, Genre = new Genre {Name = "Public criticism" } },
                new() { Id = 12, Title = "Európa elrablása", Author = new Author { Name = "Márai Sándor"}, Genre = new Genre {Name = "War criticism" } },
            }.AsQueryable();

            _context.AddRange(books);
            _context.SaveChanges();
        }

        private void ResetDatabase()
        {
            _context.Set<Book>().RemoveRange(_context.Set<Book>());
            _context.Set<Author>().RemoveRange(_context.Set<Author>());
            _context.Set<Genre>().RemoveRange(_context.Set<Genre>());
            _context.SaveChanges();
        }

        [Fact]
        public void GetBooks_ShouldReturnAllBooks_WhenNoFiltersAreApplied()
        {
            var filterOptions = new BookFilterOptions { };

            var result = _repository.GetBooks(filterOptions).ToList();

            result.Should().HaveCount(_context.Set<Book>().Count());
        }

        [Fact]
        public void GetBooks_ShouldIncludeAuthor_WhenIncludeAuthorIsTrue()
        {
            var filterOptions = new BookFilterOptions { IncludeAuthor = true };

            var result = _repository.GetBooks(filterOptions).ToList();

            Assert.NotNull(result);
            Assert.True(result.All(b => b.Author is not null));
        }

        [Fact]
        public void GetBooks_ShouldIncludeGenre_WhenIncludeGenreIsTrue()
        {
            var filterOptions = new BookFilterOptions { IncludeGenre = true};

            var result = _repository.GetBooks(filterOptions).ToList();

            Assert.NotNull(result);
            Assert.True(result.All(b => b.Genre is not null));
        }

        [Theory]
        [InlineData("title", "1984")]
        public void GetBooks_ShouldFilterByTitle_WhenFilterByIsTitle(string title, string query)
        {
            var filterOptions = new BookFilterOptions { FilterBy = title, Query = query };

            var result = _repository.GetBooks(filterOptions).ToList();

            result.Should().ContainSingle(b => b.Title == query);
        }

        [Theory]
        [InlineData("title", "Európa elrablása")]
        public void GetBooks_ShouldFilterByTitleMultiple_WhenFilterByIsTitle(string title, string query)
        {
            var filterOptions = new BookFilterOptions { FilterBy = title, Query = query };

            var result = _repository.GetBooks(filterOptions).ToList();

            result.Should().OnlyContain(b => b.Title == query);
        }

        [Theory]
        [InlineData("author", "J.R.R. Tolkien")]
        public void GetBooks_ShouldFilterByAuthor_WhenFilterByIsAuthor(string author, string query)
        {
            var filterOptions = new BookFilterOptions { FilterBy = author, Query = query };

            var result = _repository.GetBooks(filterOptions).ToList();

            result.Should().ContainSingle(b => b.Author!.Name == query);
        }

        [Theory]
        [InlineData("author", "J.K. Rowling")]
        public void GetBooks_ShouldFilterByAuthorMultiple_WhenFilterByIsAuthor(string author, string query)
        {
            var filterOptions = new BookFilterOptions { FilterBy = author, Query = query };

            var result = _repository.GetBooks(filterOptions).ToList();

            result.Should().OnlyContain(b => b.Author!.Name == query);
        }

        [Theory]
        [InlineData("author", "Historical Fiction")]
        public void GetBooks_ShouldFilterByGenre_WhenFilterByIsGenre(string genre, string query)
        {
            var filterOptions = new BookFilterOptions { FilterBy = genre, Query = query };

            var result = _repository.GetBooks(filterOptions).ToList();

            result.Should().ContainSingle(b => b.Genre!.Name == query);
        }

        [Theory]
        [InlineData("author", "Mystery")]
        public void GetBooks_ShouldFilterByGenreMultiple_WhenFilterByIsGenre(string genre, string query)
        {
            var filterOptions = new BookFilterOptions { FilterBy = genre, Query = query };

            var result = _repository.GetBooks(filterOptions).ToList();

            result.Should().OnlyContain(b => b.Genre!.Name == query);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(8)]
        public async Task GetBookDetailsByIdAsync_ShouldReturnBook_WhenBookExists(int bookId)
        {
            var expectedBook = await _context.Set<Book>().FirstOrDefaultAsync(b => b.Id == bookId);

            var result = await _repository.GetBookDetailsByIdAsync(bookId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedBook);
        }

        [Theory]
        [InlineData(42)]
        public async Task GetBookDetailsByIdAsync_ShouldReturnNull_WhenBookDoesExist(int bookId)
        {
            var result = await _repository.GetBookDetailsByIdAsync(bookId);

            result.Should().BeNull();
        }

        [Fact] //Service
        public async Task GetBookDetailsByIdAsync_ShouldThrowException_WhenBookIsInvalid()
        {
            int invalidBookId = -1;

            Func<Task> act = async () => await _repository.GetBookDetailsByIdAsync(invalidBookId);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact] //Service
        public void GetBooks_ShouldOrderByTitle_WhenOrderByIsImplicitlyTitle()
        {
            var filterOptions = new BookFilterOptions { };
            var result = _repository.GetBooks(filterOptions).ToList();

            result.Should().BeInAscendingOrder(b => b.Title, StringComparer.CurrentCulture);
        }

        [Fact] //Service
        public void GetBooks_ShouldOrderByTitle_WhenOrderByIsExplicitlyTitle()
        {
            var filterOptions = new BookFilterOptions { OrderBy = "title" };
            var result = _repository.GetBooks(filterOptions).ToList();

            result.Should().BeInAscendingOrder(b => b.Title, StringComparer.CurrentCulture);
        }

        //[Fact]
        //public void GetBooks_ShouldReturnAllBooks_WhenFilterByIsInvalid()
        //{
        //    var filterOptions = new FilterOptions { IncludeAuthor = false, IncludeGenre = false, Query = "", FilterBy = "invalid" };
        //    var result = _repository.GetBooks(filterOptions).ToList();

        //    result.Should().HaveCount(8);
        //}
    }
}
