using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Settlements.Server.Data;
using System.Data.Common;

namespace Settlements.UnitTests.Utility
{
	// Based on SampleDbContextFactory from:
	// https://www.meziantou.net/testing-ef-core-in-memory-using-sqlite.htm
	public sealed class SettlementsContextFactory : IDisposable
	{
		private DbConnection? _connection;

		private DbContextOptions<SettlementsContext> CreateOptions()
		{
			return new DbContextOptionsBuilder<SettlementsContext>().UseSqlite(_connection!).Options;
		}

		public SettlementsContext CreateContext()
		{
			if (_connection is null)
			{
				_connection = new SqliteConnection("DataSource=:memory:");
				_connection.Open();

				var options = CreateOptions();
				using var context = new SettlementsContext(options);
				context.Database.EnsureCreated();
			}

			return new SettlementsContext(CreateOptions());
		}

		public void Dispose()
		{
			if (_connection is not null)
			{
				_connection.Dispose();
				_connection = null;
			}
		}
	}
}
