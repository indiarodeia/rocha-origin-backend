using Api.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

[Table("order_item")]
public sealed class OrderItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }

    public Guid? ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    [Required]
    [MaxLength(200)]
    public string ProductName { get; set; } = string.Empty;

    public Guid? EstablishmentMenuItemId { get; set; }

    [ForeignKey(nameof(EstablishmentMenuItemId))]
    public EstablishmentMenuItem? EstablishmentMenuItem { get; set; }

    [Required]
    [Column(TypeName = "numeric(10,3)")]
    public decimal RequestedQuantity { get; set; }

    [Required]
    public int RequestedUnitId { get; set; } = (int)ProductUnitEnum.KG;

    [ForeignKey(nameof(RequestedUnitId))]
    public ProductUnit? RequestedUnit { get; set; }

    [Column(TypeName = "numeric(10,3)")]
    public decimal? ApproxKgPerUnit { get; set; }

    [MaxLength(1000)]
    public string? RequestNotes { get; set; }

    [Required]
    [Column(TypeName = "numeric(18,2)")]
    public decimal UnitPrice { get; set; }

    [Required]
    public int PriceUnitId { get; set; } = (int)ProductUnitEnum.KG;

    [ForeignKey(nameof(PriceUnitId))]
    public ProductUnit? PriceUnit { get; set; }

    [Column(TypeName = "numeric(10,3)")]
    public decimal? PreparedQuantity { get; set; }

    public int? PreparedUnitId { get; set; }

    [ForeignKey(nameof(PreparedUnitId))]
    public ProductUnit? PreparedUnit { get; set; }

    [Column(TypeName = "numeric(10,3)")]
    public decimal? PreparedWeightKg { get; set; }

    [Required]
    public int TraceabilitySourceTypeId { get; set; } = (int)TraceabilitySourceTypeEnum.None;

    [ForeignKey(nameof(TraceabilitySourceTypeId))]
    public TraceabilitySourceType? TraceabilitySourceType { get; set; }

    public Guid? AnimalId { get; set; }

    [ForeignKey(nameof(AnimalId))]
    public Animal? Animal { get; set; }

    public Guid? LotId { get; set; }

    [ForeignKey(nameof(LotId))]
    public Lot? Lot { get; set; }

    [Required]
    public int OrderStatusId { get; set; } = (int)OrderStatusEnum.Pending;

    [ForeignKey(nameof(OrderStatusId))]
    public OrderStatus? Status { get; set; }

    public DateTime? PreparedAt { get; set; }

    public Guid? PreparedByUserId { get; set; }

    [MaxLength(1000)]
    public string? PrepNotes { get; set; }
}