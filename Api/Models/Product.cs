using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

[Table("product")]
public sealed class Product
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int ProductCategoryId { get; set; }

    [ForeignKey(nameof(ProductCategoryId))]
    public ProductCategory? Category { get; set; }

    [Required]
    public int DefaultUnitId { get; set; }

    [ForeignKey(nameof(DefaultUnitId))]
    public ProductUnit? DefaultUnit { get; set; }

    public decimal? DefaultVatRate { get; set; }

    [Column(TypeName = "numeric(18,2)")]
    public decimal? DefaultSellPrice { get; set; }

    [MaxLength(50)]
    public string? InternalCode { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}