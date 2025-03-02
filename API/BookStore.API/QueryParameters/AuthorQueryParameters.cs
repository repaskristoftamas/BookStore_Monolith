namespace BookStore.API.QueryParameters
{
    public class AuthorQueryParameters : PaginationParameters
    {
        public string? Name { get; set; }
        public string? BookTitle { get; set; }
        public string? SearchQuery { get; set; }
        public string? OrderBy { get; set; } = "name";

        public void Deconstruct(out string? name, out string? bookTitle, out string? searchQuery)
        {
            name = Name;
            bookTitle = BookTitle;
            searchQuery = SearchQuery;
        }
    }
}
