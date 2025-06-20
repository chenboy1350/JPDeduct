namespace Deduct.Models
{
    public class PagedListModel<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T> Items { get; set; } = [];
        public DeductFilterModel DeductFilter { get; set; } = new DeductFilterModel();
        public HDeductFilterModel HDeductFilter { get; set; } = new HDeductFilterModel();
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public PagedListModel()
        {

        }
    }
}