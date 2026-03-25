using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

[Table("lot_animal")]
public sealed class LotAnimal
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid LotId { get; set; }

    [ForeignKey(nameof(LotId))]
    public Lot? Lot { get; set; }

    [Required]
    public Guid AnimalId { get; set; }

    [ForeignKey(nameof(AnimalId))]
    public Animal? Animal { get; set; }

    [Required]
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}