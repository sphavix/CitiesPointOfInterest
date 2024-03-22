using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CityPointOfInterestApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CityName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PointsOfInterest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PointOfInterestName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    CityId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointsOfInterest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointsOfInterest_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CityName", "Description" },
                values: new object[,]
                {
                    { 1, "New York City", "The one with that big park." },
                    { 2, "Antwerp", "The one with the cathedral that was never really finished." },
                    { 3, "Paris", "The one with that big tower." }
                });

            migrationBuilder.InsertData(
                table: "PointsOfInterest",
                columns: new[] { "Id", "CityId", "Description", "PointOfInterestName" },
                values: new object[,]
                {
                    { 1, 1, "The most visited urban park in the United States.", "Central Park" },
                    { 2, 3, "A wrold-class tower on the Champ de Mars.", "Eiffel Tower" },
                    { 3, 3, "The world's largest museum.", "The Louvre" },
                    { 4, 3, "A modern museum.", "Museum of Modern Art" },
                    { 5, 2, "The most visited urban park in the United States.", "Central Park" },
                    { 6, 2, "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans.", "Cathedral" },
                    { 7, 1, "A 102-story skyscraper located in Midtown Manhattan.", "Empire State Building" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointsOfInterest_CityId",
                table: "PointsOfInterest",
                column: "CityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointsOfInterest");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
