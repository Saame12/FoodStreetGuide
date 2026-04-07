using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStreetGuide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableNameToPois : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_POIs",
                table: "POIs");

            migrationBuilder.RenameTable(
                name: "POIs",
                newName: "Pois");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pois",
                table: "Pois",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Pois",
                table: "Pois");

            migrationBuilder.RenameTable(
                name: "Pois",
                newName: "POIs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_POIs",
                table: "POIs",
                column: "Id");
        }
    }
}
