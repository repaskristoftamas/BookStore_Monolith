using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models
{
    public record class AuthorForUpdateDto(
        [Required(ErrorMessage = "Author must have a name.")]
        [MaxLength(250)]
        string Name,

        ICollection<BookDto> Books)
    {
        public AuthorForUpdateDto(string name) : this(name, []) { }
    }
}
