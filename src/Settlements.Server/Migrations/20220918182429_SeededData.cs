using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Settlements.Server.Migrations
{
    public partial class SeededData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settlements_CountryModel_CountryModelId",
                table: "Settlements");

            migrationBuilder.DropTable(
                name: "CountryModel");

            migrationBuilder.DropIndex(
                name: "IX_Settlements_CountryModelId",
                table: "Settlements");

            migrationBuilder.RenameColumn(
                name: "CountryModelId",
                table: "Settlements",
                newName: "CountryId");

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    RegexPattern = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Settlements",
                newName: "CountryModelId");

            migrationBuilder.CreateTable(
                name: "CountryModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    RegexPattern = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Settlements_CountryModelId",
                table: "Settlements",
                column: "CountryModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryModel_Name",
                table: "CountryModel",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Settlements_CountryModel_CountryModelId",
                table: "Settlements",
                column: "CountryModelId",
                principalTable: "CountryModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
