using Microsoft.AspNetCore.Mvc;
using Settlements.Server.Attributes.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Settlements.Server.Data.Models
{
	public class Settlement
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[CountryExists]
		public int CountryId { get; set; }

		[Required]
		[StringLength(128, MinimumLength = 2)]
		public string Name { get; set; } = null!;

		[Required]
		[StringLength(16, MinimumLength = 4)]
		[PostalCodeBelongsToCountry("CountryId")]
		[PostalCodeIsUniqueToCountry("CountryId", "Id")]
		public string PostalCode { get; set; } = null!;
	}
}
