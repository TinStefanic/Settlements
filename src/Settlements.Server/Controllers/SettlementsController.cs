using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Settlements.Server.Data;
using Settlements.Server.Data.Models;
using Settlements.Server.Services.ValidationService;
using System.ComponentModel.DataAnnotations;

namespace Settlements.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SettlementsController : ControllerBase
	{
		private readonly SettlementsContext _context;
		private readonly ICustomSettlementValidationService _settlementValidator;

		public SettlementsController(SettlementsContext context, ICustomSettlementValidationService settlementValidator)
		{
			_context = context;
			_settlementValidator = settlementValidator;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Settlement>> GetSettlement(int id)
		{
			var settlement = await _context.Settlements.FindAsync(id);

			if (settlement is null)
			{
				return NotFound();
			}

			return Ok(settlement);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateSettlement(int id, Settlement updatedSettlement)
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
		public async Task<ActionResult<Settlement>> CreateSettlement(Settlement settlement)
		{
			_context.Settlements.Add(settlement);
			await _context.SaveChangesAsync();

			return CreatedAtAction(
				nameof(Settlement),
				new { id = settlement.Id },
				settlement
			);
		}

		[HttpDelete("{id}")]
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

		[AcceptVerbs("GET", "POST")]
		public JsonResult VerifyCountry(int countryId)
		{
			var validationResult = _settlementValidator.VerifyCountryExists(countryId);

			if (validationResult != ValidationResult.Success)
			{
				return new JsonResult(validationResult?.ErrorMessage);
			}

			return new JsonResult(true);
		}

		[AcceptVerbs("GET", "POST")]
		public JsonResult VerifyPostalCode(string postalCode, int countryId)
		{
			var validationResult = _settlementValidator.VerifyPostalCode(postalCode, countryId);

			if (validationResult != ValidationResult.Success)
			{
				return new JsonResult(validationResult?.ErrorMessage);
			}

			return new JsonResult(true);
		}

		private bool SettlementExists(int id) => _context.Settlements.Any(t => t.Id == id);
	}
}
