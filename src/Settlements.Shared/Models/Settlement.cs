using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settlements.Shared.Models
{
	public class Settlement
	{
		[Required]
		public string? Country { get; set; }

		[Required]
		public string? Name { get; set; }

		[Required]
		public string? PostalCode { get; set; }
	}
}
