using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models
{
    public record class GenreForUpdateDto(
        [Required(ErrorMessage = "Genre must have a name.")]
        [MaxLength(250)]
        string Name,

        ICollection<BookDto> Books)
    {
        public GenreForUpdateDto(string name) : this(name, []) { }
    }
}
