using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCareABApi.Migrations
{
    /// <inheritdoc />
    public partial class newinit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availabilities_Users_CaregiverId",
                table: "Availabilities");

            migrationBuilder.DropIndex(
                name: "IX_Availabilities_CaregiverId",
                table: "Availabilities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Availabilities_CaregiverId",
                table: "Availabilities",
                column: "CaregiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Availabilities_Users_CaregiverId",
                table: "Availabilities",
                column: "CaregiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
