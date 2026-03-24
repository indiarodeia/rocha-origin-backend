using Api.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    [Table("client")]
    public class Client
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string VatNumber { get; set; } = string.Empty;

        [Required]
        [Phone]
        [MaxLength(30)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; } = true;

        public Guid? BillingAddressId { get; set; }

        [ForeignKey(nameof(BillingAddressId))]
        public Address? BillingAddress { get; set; }

        [MaxLength(2000)]
        public string? Notes { get; set; }

        [Required]
        public int PaymentTypeId { get; set; } = (int)PaymentTypeEnum.Immediate;

        [ForeignKey(nameof(PaymentTypeId))]
        public PaymentType? PaymentType { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
