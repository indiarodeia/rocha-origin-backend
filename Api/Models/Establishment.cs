using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

[Table("establishment")]
public sealed class Establishment
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ClientId { get; set; }

    [ForeignKey(nameof(ClientId))]
    public Client? Client { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; } = true;

    public Guid? DeliveryAddressId { get; set; }

    [ForeignKey(nameof(DeliveryAddressId))]
    public Address? DeliveryAddress { get; set; }

    public Guid? RouteId { get; set; }

    [ForeignKey(nameof(RouteId))]
    public Route? Route { get; set; }

    [Phone]
    [MaxLength(30)]
    public string? LocalContactPhone { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<EstablishmentMenuItem> EstablishmentMenuItems { get; set; } = new List<EstablishmentMenuItem>();
}