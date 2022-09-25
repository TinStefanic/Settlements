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
	[Produces("application/json")]
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

		/// <summary>
		/// Returns paged Settlements.
		/// </summary>
		/// <param name="pageNumber"> Which page to return, starts from 1</param>
		/// <param name="pageSize">Size of page</param>
		/// <returns>Settlements page</returns>
		/// <response code="200">Returns Settlements page</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<PaginatedSettlements>> GetPaginatedSettlements(
			int pageNumber = 1,
			int pageSize = 4)
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

		/// <summary>
		/// Returns Settlement with provided id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Settlement with provided id</returns>
		/// <response code="200">Returns Settlement with provided id</response>
		/// <response code="404">Settlement with provided id doesn't exist</response>
		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Settlement>> GetSettlement(int id)
		{
			var settlement = await _context.Settlements.FindAsync(id);

			if (settlement is null)
			{
				return NotFound();
			}

			return Ok(settlement);
		}

		/// <summary>
		/// Updates Settlement with provided id.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="updatedSettlement"></param>
		/// <returns></returns>
		/// <response code="204">Settlement successfully updated</response>
		/// <response code="400">Settlement with updated values failed validation</response>
		/// <response code="404">There is no Settlement with provided id</response>
		[HttpPut("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
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

		/// <summary>
		/// Creates new Settlement.
		/// </summary>
		/// <param name="settlement"></param>
		/// <returns>Newly created Settlement</returns>
		/// <response code="201">Returns newly created item</response>
		/// <response code="400">Provided Settlement failed validation</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
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

		/// <summary>
		/// Deletes settlement with provided id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <response code="204">Settlement was successfully deleted</response>
		/// <response code="404">Settlement with provided id doesn't exist</response>
		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
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
