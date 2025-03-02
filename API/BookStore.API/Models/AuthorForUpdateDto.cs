using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models
{
    public class AuthorForUpdateDto
    {
        [Required(ErrorMessage = "Author must have a name.")]
        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;

        public ICollection<BookDto> Books { get; set; } = [];
    }
}
