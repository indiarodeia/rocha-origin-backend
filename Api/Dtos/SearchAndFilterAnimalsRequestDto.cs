namespace Api.Dtos
{
    public class SearchAndFilterAnimalsRequestDto
    {
        public string? Search { get; set; }

        public int? SpeciesId { get; set; }

        public Guid? SupplierId { get; set; }

        public bool IsSortAscending { get; set; } = true;

        public string? SortBy { get; set; } = "slaughterdate";

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}