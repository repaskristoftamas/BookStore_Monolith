namespace BookStore.API.Helpers
{
    public static class ValidationHelper
    {
        public static void ValidatePagination(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Page number must be greater than 0.");

            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than 0.");
        }
    }
}
