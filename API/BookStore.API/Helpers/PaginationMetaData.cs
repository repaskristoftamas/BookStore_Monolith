﻿namespace BookStore.API.Helpers
{
    public class PaginationMetaData(int totalItemCount, int pageSize, int currentPage)
    {
        public int TotalItemCount { get; set; } = totalItemCount;
        public int TotalPageCount { get; set; } = (int)Math.Ceiling(totalItemCount / (double)pageSize);
        public int PageSize { get; set; } = pageSize;
        public int CurrentPage { get; set; } = currentPage;
    }
}
