using System.ComponentModel.DataAnnotations;

namespace Settlements.Server.Data.Models
{
    public class Settlement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string PostalCode { get; set; } = null!;
    }
}
