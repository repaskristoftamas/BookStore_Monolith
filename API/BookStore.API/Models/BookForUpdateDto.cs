using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models
{
    public record class BookForUpdateDto(
        [Required(ErrorMessage = "Book must have a title.")]
        [MaxLength(250)]
        string Title,

        [MaxLength(2500)]
        string? Description,

        [Required(ErrorMessage = "Book must have an author.")]
        AuthorDto? Author,

        [Required(ErrorMessage = "Book must have a genre.")]
        GenreDto? Genre
    );
}
