using Microsoft.EntityFrameworkCore;
using Settlements.Server.Data.Models;

namespace Settlements.Server.Data
{
	public class SettlementsContext : DbContext
	{
		public SettlementsContext(DbContextOptions<SettlementsContext> options) : base(options)
		{
		}

		public DbSet<Settlement> Settlements { get; set; } = null!;
		public DbSet<Country> Countries { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique(true);
		}
	}
}
