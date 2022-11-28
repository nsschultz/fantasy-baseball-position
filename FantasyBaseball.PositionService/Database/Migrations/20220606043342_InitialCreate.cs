using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyBaseball.PositionService.Database.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    FullName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PlayerType = table.Column<int>(type: "integer", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Position_PK", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "AdditionalPositions",
                columns: table => new
                {
                    ParentCode = table.Column<string>(type: "character varying(4)", nullable: false),
                    ChildCode = table.Column<string>(type: "character varying(4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AdditionalPosition_PK", x => new { x.ParentCode, x.ChildCode });
                    table.ForeignKey(
                        name: "AdditionalPosition_ChildPosition_FK",
                        column: x => x.ChildCode,
                        principalTable: "Positions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "AdditionalPosition_ParentPosition_FK",
                        column: x => x.ParentCode,
                        principalTable: "Positions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "Code", "FullName", "PlayerType", "SortOrder" },
                values: new object[,]
                {
                    { "", "Unknown", 0, 2147483647 },
                    { "1B", "First Baseman", 1, 1 },
                    { "2B", "Second Baseman", 1, 2 },
                    { "3B", "Third Baseman", 1, 3 },
                    { "C", "Catcher", 1, 0 },
                    { "CF", "Center Feilder", 1, 9 },
                    { "CIF", "Corner Infielder", 1, 5 },
                    { "DH", "Designated Hitter", 1, 12 },
                    { "IF", "Infielder", 1, 7 },
                    { "LF", "Left Fielder", 1, 8 },
                    { "MIF", "Middle Infielder", 1, 6 },
                    { "OF", "Outfielder", 1, 11 },
                    { "P", "Pitcher", 2, 102 },
                    { "RF", "Right Fielder", 1, 10 },
                    { "RP", "Relief Pitcher", 2, 101 },
                    { "SP", "Starting Pitcher", 2, 100 },
                    { "SS", "Shortstop", 1, 4 },
                    { "UTIL", "Utility", 1, 13 }
                });

            migrationBuilder.InsertData(
                table: "AdditionalPositions",
                columns: new[] { "ChildCode", "ParentCode" },
                values: new object[,]
                {
                    { "CIF", "1B" },
                    { "IF", "1B" },
                    { "UTIL", "1B" },
                    { "IF", "2B" },
                    { "MIF", "2B" },
                    { "UTIL", "2B" },
                    { "CIF", "3B" },
                    { "IF", "3B" },
                    { "UTIL", "3B" },
                    { "UTIL", "C" },
                    { "OF", "CF" },
                    { "UTIL", "CF" },
                    { "IF", "CIF" },
                    { "UTIL", "CIF" },
                    { "UTIL", "DH" },
                    { "UTIL", "IF" },
                    { "OF", "LF" },
                    { "UTIL", "LF" },
                    { "IF", "MIF" },
                    { "UTIL", "MIF" },
                    { "UTIL", "OF" },
                    { "OF", "RF" },
                    { "UTIL", "RF" },
                    { "P", "RP" },
                    { "P", "SP" },
                    { "IF", "SS" },
                    { "MIF", "SS" },
                    { "UTIL", "SS" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalPositions_ChildCode",
                table: "AdditionalPositions",
                column: "ChildCode");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_SortOrder",
                table: "Positions",
                column: "SortOrder",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalPositions");

            migrationBuilder.DropTable(
                name: "Positions");
        }
    }
}
