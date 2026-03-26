namespace Api.Dtos
{
    public class SearchAndFilterProductsRequestDto
    {
        public string? Search { get; set; }

        public int? CategoryId { get; set; }

        public decimal? VatRate { get; set; }

        public bool? IsActive { get; set; }

        public bool IsSortAscending { get; set; } = true;

        public string? SortBy { get; set; } = "name";

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}