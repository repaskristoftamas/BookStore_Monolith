using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models
{
    public class BookForCreationDto
    {
        [Required(ErrorMessage = "Book must have a title.")]
        [MaxLength(250)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2500)]
        public string? Description { get; set; }
        public AuthorDto? Author { get; set; }
        public GenreDto? Genre { get; set; }
    }
}
