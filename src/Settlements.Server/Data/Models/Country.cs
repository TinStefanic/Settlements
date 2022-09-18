using System.ComponentModel.DataAnnotations;

namespace Settlements.Server.Data.Models
{
	public class Country
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(128, MinimumLength = 2)]
		public string Name { get; set; } = null!;

		[Required]
		public string RegexPattern { get; set; } = null!;
	}
}
