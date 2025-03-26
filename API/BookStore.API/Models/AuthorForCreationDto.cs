using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models
{
    public record class AuthorForCreationDto(
        [Required(ErrorMessage = "Author must have a name.")]
        [MaxLength(250)]
        string Name
    );
}
