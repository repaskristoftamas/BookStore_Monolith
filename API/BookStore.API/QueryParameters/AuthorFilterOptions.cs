namespace BookStore.API.QueryParameters
{
    public class AuthorFilterOptions// : PaginationParameters
    {
        public string? Query { get; set; }
        public string? OrderBy { get; set; } = "name";
    }
}
