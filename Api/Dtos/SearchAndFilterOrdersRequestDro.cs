namespace Api.Dtos
{
    public class SearchAndFilterOrdersRequestDto
    {
        public string? Search { get; set; }

        public List<int>? StatusIds { get; set; }

        public List<Guid>? RouteIds { get; set; }

        public int? DeliveryTypeId { get; set; }

        public bool HideDelivered { get; set; } = false;

        public bool IsSortAscending { get; set; } = true;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}