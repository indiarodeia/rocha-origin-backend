using Api.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

[Table("order")]
public class Order
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ClientId { get; set; }

    [ForeignKey(nameof(ClientId))]
    public Client? Client { get; set; }

    public Guid? EstablishmentId { get; set; }

    [ForeignKey(nameof(EstablishmentId))]
    public Establishment? Establishment { get; set; }

    [MaxLength(200)]
    public string? QuickClientName { get; set; }

    [Required]
    public int OrderStatusId { get; set; } = (int)OrderStatusEnum.Pending;

    [ForeignKey(nameof(OrderStatusId))]
    public OrderStatus? Status { get; set; }

    public DateTime? PrepDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public TimeSpan? DeliveryDeadlineTime { get; set; }

    [Required]
    public int DeliveryTypeId { get; set; } = (int)DeliveryTypeEnum.Delivery;

    [ForeignKey(nameof(DeliveryTypeId))]
    public DeliveryType? DeliveryType { get; set; }

    [Required]
    public bool IsUrgent { get; set; } = false;

    [MaxLength(100)]
    public string? OrderCategory { get; set; }

    public Guid? RouteId { get; set; }

    [ForeignKey(nameof(RouteId))]
    public Route? Route { get; set; }

    [Required]
    public int PaymentTypeId { get; set; } = (int)PaymentTypeEnum.Immediate;

    [ForeignKey(nameof(PaymentTypeId))]
    public PaymentType? PaymentType { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }

    public Guid? CreatedByUserId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}