using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models
{
    public record class GenreForCreationDto(
        [Required(ErrorMessage = "Genre must have a name.")]
        [MaxLength(250)]
        string Name
    );
}
