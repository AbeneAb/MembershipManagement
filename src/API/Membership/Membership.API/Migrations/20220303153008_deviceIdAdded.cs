using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Membership.API.Migrations
{
    public partial class deviceIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "HealthInformation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "HealthInformation");
        }
    }
}
