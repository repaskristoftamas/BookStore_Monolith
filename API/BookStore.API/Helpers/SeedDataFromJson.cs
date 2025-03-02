using BookStore.API.DbContexts;
using BookStore.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BookStore.API.Helpers
{
    public static class SeedDataFromJson
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();

            await context.Database.EnsureCreatedAsync();

            var data = JsonSerializer.Deserialize<Dictionary<string, List<object>>>(
                await File.ReadAllTextAsync("DummyData.json"))!;

            if (!await context.Set<Genre>().AnyAsync())
            {
                var genres = JsonSerializer.Deserialize<List<Genre>>(JsonSerializer.Serialize(data["genres"]))!;
                context.Set<Genre>().AddRange(genres);
                await context.SaveChangesAsync();
            }

            if (!await context.Set<Author>().AnyAsync())
            {
                var authors = JsonSerializer.Deserialize<List<Author>>(JsonSerializer.Serialize(data["authors"]))!;
                context.Set<Author>().AddRange(authors);
                await context.SaveChangesAsync();
            }

            if (!await context.Set<Book>().AnyAsync())
            {
                var books = JsonSerializer.Deserialize<List<Book>>(JsonSerializer.Serialize(data["books"]))!;

                foreach (var book in books)
                {
                    var author = await context.Set<Author>().FirstOrDefaultAsync(a => a.Id == book.AuthorId);
                    var genre = await context.Set<Genre>().FirstOrDefaultAsync(g => g.Id == book.GenreId);

                    if (author == null || genre == null) continue;

                    book.Author = author;
                    book.Genre = genre;

                    context.Set<Book>().Add(book);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
