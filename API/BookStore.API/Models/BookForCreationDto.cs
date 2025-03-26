using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models
{
    public record class BookForCreationDto(
        [Required(ErrorMessage = "Book must have a title.")]
        [MaxLength(250)]
        string Title,

        [MaxLength(2500)]
        string? Description,

        AuthorForBookCreationDto Author,

        GenreDto? Genre
    );
}
