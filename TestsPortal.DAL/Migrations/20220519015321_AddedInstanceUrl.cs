using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestsPortal.DAL.Migrations
{
    public partial class AddedInstanceUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminPanelInstance",
                table: "ScheduledTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminPanelInstance",
                table: "ScheduledTests");
        }
    }
}
