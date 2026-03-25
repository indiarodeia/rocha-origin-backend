using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

[Table("animal")]
public class Animal
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public int AnimalSpeciesId { get; set; }

    [ForeignKey(nameof(AnimalSpeciesId))]
    public AnimalSpecies? Species { get; set; }

    [Required]
    [MaxLength(100)]
    public string AnimalIdentification { get; set; } = string.Empty;

    [Required]
    public DateTime SlaughterDate { get; set; }

    public DateTime? DispatchDate { get; set; }

    public DateTime? ArrivalDate { get; set; }

    [MaxLength(200)]
    public string? BirthPlace { get; set; }

    [MaxLength(200)]
    public string? RearingPlace { get; set; }

    [MaxLength(100)]
    public string? Breed { get; set; }

    [Required]
    public Guid SupplierId { get; set; }

    [ForeignKey(nameof(SupplierId))]
    public Supplier? Supplier { get; set; }

    [Required]
    [Column(TypeName = "numeric(10,3)")]
    public decimal ColdWeightKg { get; set; }

    public int? AgeMonths { get; set; }

    [Column(TypeName = "numeric(4,2)")]
    public decimal? Ph { get; set; }

    [MaxLength(1)]
    public string? EuropConformation { get; set; }

    public int? EuropFatClass { get; set; }

    [MaxLength(1)]
    public string? EuropCategory { get; set; }

    [MaxLength(100)]
    public string? EuropRaw { get; set; }

    [MaxLength(500)]
    public string? CarcassPhotoUrl { get; set; }

    [MaxLength(100)]
    public string? SlaughterhouseRef { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;
}