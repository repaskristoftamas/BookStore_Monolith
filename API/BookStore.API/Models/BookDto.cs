namespace BookStore.API.Models
{
    public record class BookDto(int Id, string Title, string? Description, AuthorDto? Author, GenreDto? Genre);
}
