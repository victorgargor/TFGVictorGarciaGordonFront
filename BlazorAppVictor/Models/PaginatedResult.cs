namespace BlazorAppVictor.Models
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public int PageSize { get; set; } = 5;
        public int CurrentPage { get; set; }

        // Constructor
        public PaginatedResult(List<T> items, int totalCount, int currentPage, int pageSize = 5)
        {
            Items = items;
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }
}
