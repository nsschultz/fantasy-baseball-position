using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyBaseball.PositionService.Database.Migrations
{
    /// <inheritdoc />
    public partial class CfSpellingFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "Code",
                keyValue: "CF",
                column: "FullName",
                value: "Center Fielder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "Code",
                keyValue: "CF",
                column: "FullName",
                value: "Center Feilder");
        }
    }
}
