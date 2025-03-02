namespace BookStore.API.Models
{
    public class AuthorWithoutBooksDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
