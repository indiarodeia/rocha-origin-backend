namespace Api.Dtos
{
    public class SearchAndFilterSuppliersRequestDto
    {
        public string? Search { get; set; }

        public string? City { get; set; }

        public bool IsSortAscending { get; set; } = true;

        public string? SortBy { get; set; } = "name";

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}