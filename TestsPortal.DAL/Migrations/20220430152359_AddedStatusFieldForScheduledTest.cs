using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestsPortal.DAL.Migrations
{
    public partial class AddedStatusFieldForScheduledTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "Status",
                table: "ScheduledTests",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ScheduledTests");
        }
    }
}
