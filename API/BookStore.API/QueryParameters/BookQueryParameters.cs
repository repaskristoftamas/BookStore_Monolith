namespace BookStore.API.QueryParameters
{
    public class BookQueryParameters : PaginationParameters
    {
        public bool IncludeAuthor { get; set; }
        public bool IncludeGenre { get; set; }
        public string? Query { get; set; }
        public string? FilterBy { get; set; }
        public string? OrderBy { get; set; } = "title";

        public void Deconstruct( out bool includeAuthor, out bool includeGenre, out string? query, out string? filterBy)
        {
            includeAuthor = IncludeAuthor;
            includeGenre = IncludeGenre;
            query = Query;
            filterBy = FilterBy;
        }
    }
}
