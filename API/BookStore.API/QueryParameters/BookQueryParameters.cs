namespace BookStore.API.QueryParameters
{
    public class BookQueryParameters : PaginationParameters
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public bool IncludeAuthor { get; set; }
        public bool IncludeGenre { get; set; }
        public string? SearchQuery { get; set; }
        public string? OrderBy { get; set; } = "title";

        public void Deconstruct(out string? title, out string? author, out string? genre, out bool includeAuthor, out bool includeGenre, out string? searchQuery)
        {
            title = Title;
            author = Author;
            genre = Genre;
            includeAuthor = IncludeAuthor;
            includeGenre = IncludeGenre;
            searchQuery = SearchQuery;
        }
    }
}
