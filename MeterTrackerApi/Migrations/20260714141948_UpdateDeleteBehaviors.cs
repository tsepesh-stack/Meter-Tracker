using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeterTrackerApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehaviors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Premises_Users_ResponsibleUserId",
                table: "Premises");

            migrationBuilder.DropForeignKey(
                name: "FK_Readings_Users_SubmittedById",
                table: "Readings");

            migrationBuilder.AlterColumn<int>(
                name: "ResponsibleUserId",
                table: "Premises",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Premises_Users_ResponsibleUserId",
                table: "Premises",
                column: "ResponsibleUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Readings_Users_SubmittedById",
                table: "Readings",
                column: "SubmittedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Premises_Users_ResponsibleUserId",
                table: "Premises");

            migrationBuilder.DropForeignKey(
                name: "FK_Readings_Users_SubmittedById",
                table: "Readings");

            migrationBuilder.AlterColumn<int>(
                name: "ResponsibleUserId",
                table: "Premises",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Premises_Users_ResponsibleUserId",
                table: "Premises",
                column: "ResponsibleUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Readings_Users_SubmittedById",
                table: "Readings",
                column: "SubmittedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
