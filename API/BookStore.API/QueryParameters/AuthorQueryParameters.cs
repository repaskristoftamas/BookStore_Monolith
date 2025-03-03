namespace BookStore.API.QueryParameters
{
    public class AuthorQueryParameters : PaginationParameters
    {
        public string? Query { get; set; }
        public string? OrderBy { get; set; } = "name";
    }
}
