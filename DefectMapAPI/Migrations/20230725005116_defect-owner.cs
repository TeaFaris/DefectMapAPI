using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefectMapAPI.Migrations
{
    /// <inheritdoc />
    public partial class defectowner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Defects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Defects_OwnerId",
                table: "Defects",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_Users_OwnerId",
                table: "Defects",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defects_Users_OwnerId",
                table: "Defects");

            migrationBuilder.DropIndex(
                name: "IX_Defects_OwnerId",
                table: "Defects");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Defects");
        }
    }
}
