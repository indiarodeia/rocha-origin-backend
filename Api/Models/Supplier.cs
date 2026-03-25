using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

[Table("supplier")]
public class Supplier
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? VatNumber { get; set; }

    [Required]
    [Column(TypeName = "numeric(5,2)")]
    public decimal VatRate { get; set; }

    [MaxLength(100)]
    public string? ExplorationId { get; set; }

    public Guid? AddressId { get; set; }

    [ForeignKey(nameof(AddressId))]
    public Address? Address { get; set; }

    [MaxLength(500)]
    public string? ProfilePicture { get; set; }

    [MaxLength(500)]
    public string? CoverPicture { get; set; }

    [Required]
    [Phone]
    [MaxLength(30)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Certifications { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

    [Required]
    public bool IsActive { get; set; } = true;
}