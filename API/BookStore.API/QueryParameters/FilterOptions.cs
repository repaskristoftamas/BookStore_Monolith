namespace BookStore.API.QueryParameters
{
    public class FilterOptions// : PaginationParameters
    {
        public bool IncludeAuthor { get; set; }
        public bool IncludeGenre { get; set; }
        public string? Query { get; set; }
        public string? FilterBy { get; set; }
        public string? OrderBy { get; set; } = "title";
    }
}
