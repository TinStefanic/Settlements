using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Settlements.Server.Data;
using Settlements.Server.Data.Models;

namespace Settlements.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CountriesController : ControllerBase
	{
		private readonly SettlementsContext _context;

		public CountriesController(SettlementsContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
		{
			return await _context.Countries.ToListAsync();
		}
	}
}
