using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherSideServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "ForecastDays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ForecastDays_CityId",
                table: "ForecastDays",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForecastDays_Cities_CityId",
                table: "ForecastDays",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForecastDays_Cities_CityId",
                table: "ForecastDays");

            migrationBuilder.DropIndex(
                name: "IX_ForecastDays_CityId",
                table: "ForecastDays");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "ForecastDays");
        }
    }
}
