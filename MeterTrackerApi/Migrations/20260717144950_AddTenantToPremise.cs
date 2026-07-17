using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeterTrackerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantToPremise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantName",
                table: "Premises",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantName",
                table: "Premises");
        }
    }
}
