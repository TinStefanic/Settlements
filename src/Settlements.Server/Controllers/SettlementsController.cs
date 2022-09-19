using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Settlements.Server.Data;
using Settlements.Server.Data.Models;
using Settlements.Server.DTOs;
using Settlements.Server.Services.ValidationService;

namespace Settlements.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SettlementsController : ControllerBase
	{
		private int PageSize { get; }

		private readonly SettlementsContext _context;
		private readonly ICustomSettlementValidationService _settlementValidator;
		private readonly IConfiguration _configuration;

		public SettlementsController(
			SettlementsContext context, 
			ICustomSettlementValidationService settlementValidator,
			IConfiguration configuration)
		{
			_context = context;
			_settlementValidator = settlementValidator;
			_configuration = configuration;

			PageSize = _configuration.GetValue("Pagination:PageSize", 4);
		}

		[HttpGet]
		public async Task<ActionResult<PaginatedSettlements>> GetPaginatedSettlements(
			int pageNumber = 1,
			int pageSize = 0)
		{
			if (pageNumber < 1) pageNumber = 1;
			if (pageSize <= 0) pageSize = PageSize;

			var paginatedSettlements = 
				await _context.Settlements.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
			int totalSettlementsCount = await _context.Settlements.CountAsync();

			return new PaginatedSettlements
			{
				PageNumber = pageNumber,
				PageSize = pageSize,
				Settlements = paginatedSettlements,
				TotalSettlementsCount = totalSettlementsCount
			};
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<Settlement>> GetSettlement(int id)
		{
			var settlement = await _context.Settlements.FindAsync(id);

			if (settlement is null)
			{
				return NotFound();
			}

			return Ok(settlement);
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateSettlement([FromRoute] int id, [FromBody] Settlement updatedSettlement)
		{
			if (id != updatedSettlement.Id)
			{
				return BadRequest();
			}

			var settlement = await _context.Settlements.FindAsync(id);

			if (settlement is null)
			{
				return NotFound();
			}

			settlement.PostalCode = updatedSettlement.PostalCode;
			settlement.Name = updatedSettlement.Name;
			settlement.CountryId = updatedSettlement.CountryId;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException) when (!SettlementExists(id))
			{
				return NotFound();
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<Settlement>> CreateSettlement([FromBody] Settlement settlement)
		{
			_context.Settlements.Add(settlement);
			await _context.SaveChangesAsync();

			return CreatedAtAction(
				nameof(GetSettlement),
				new { id = settlement.Id },
				settlement
			);
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteSettlement(int id)
		{
			var settlement = await _context.Settlements.FindAsync(id);

			if (settlement is null)
			{
				return NotFound();
			}

			_context.Settlements.Remove(settlement);
			await _context.SaveChangesAsync();

			return NoContent();
		}
		private bool SettlementExists(int id) => _context.Settlements.Any(t => t.Id == id);
	}
}
