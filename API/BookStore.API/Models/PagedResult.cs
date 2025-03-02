using BookStore.API.Helpers;

namespace BookStore.API.Models
{
    public class PagedResult<T>(IEnumerable<T> items, PaginationMetaData paginationMetaData)
    {
        public IEnumerable<T> Items { get; set; } = items;
        public PaginationMetaData Pagination { get; set; } = paginationMetaData;
    }
}
