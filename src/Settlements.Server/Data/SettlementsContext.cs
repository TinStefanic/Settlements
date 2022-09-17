using Microsoft.EntityFrameworkCore;
using Settlements.Shared.Models;

namespace Settlements.Server.Data
{
	public class SettlementsContext : DbContext
	{
		public DbSet<Settlement> Settlements { get; set; } 

		public SettlementsContext(DbContextOptions<SettlementsContext> options) : base(options)
		{
		}
	}
}
