using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestsDelivery.DAL.Migrations
{
    public partial class AddedCandidateInScheduledTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "CandidatesInScheduledTest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidateId = table.Column<long>(type: "bigint", nullable: false),
                    ScheduledTestId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidatesInScheduledTest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidatesInScheduledTest_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidatesInScheduledTest_ScheduledTests_ScheduledTestId",
                        column: x => x.ScheduledTestId,
                        principalTable: "ScheduledTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidatesInScheduledTest_CandidateId",
                table: "CandidatesInScheduledTest",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidatesInScheduledTest_ScheduledTestId",
                table: "CandidatesInScheduledTest",
                column: "ScheduledTestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidatesInScheduledTest");

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
    }
}
