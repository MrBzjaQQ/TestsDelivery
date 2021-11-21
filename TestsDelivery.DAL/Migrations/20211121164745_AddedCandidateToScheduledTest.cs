using Microsoft.EntityFrameworkCore.Migrations;

namespace TestsDelivery.DAL.Migrations
{
    public partial class AddedCandidateToScheduledTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CandidateId",
                table: "ScheduledTests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledTests_CandidateId",
                table: "ScheduledTests",
                column: "CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledTests_Candidates_CandidateId",
                table: "ScheduledTests",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledTests_Candidates_CandidateId",
                table: "ScheduledTests");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledTests_CandidateId",
                table: "ScheduledTests");

            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "ScheduledTests");
        }
    }
}
