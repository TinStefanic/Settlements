using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Settlements.Server.Data;
using Settlements.Server.Data.Models;
using Settlements.Shared.DTOs;

namespace Settlements.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Produces("application/json")]
	public class CountriesController : ControllerBase
	{
		private readonly SettlementsContext _context;

		public CountriesController(SettlementsContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Returns all countries.
		/// </summary>
		/// <returns></returns>
		/// <response code="200">Returns all countries</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<CountryDTO>>> GetCountries()
		{
			return await _context.Countries.Select(c => c.Adapt<CountryDTO>()).ToListAsync();
		}
	}
}
