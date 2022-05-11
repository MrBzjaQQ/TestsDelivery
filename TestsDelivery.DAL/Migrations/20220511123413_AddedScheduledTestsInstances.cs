using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestsDelivery.DAL.Migrations
{
    public partial class AddedScheduledTestsInstances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidatesInScheduledTest");

            migrationBuilder.DropColumn(
                name: "Keycode",
                table: "ScheduledTests");

            migrationBuilder.DropColumn(
                name: "Pin",
                table: "ScheduledTests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ScheduledTests");

            migrationBuilder.CreateTable(
                name: "ChoiceAnswers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerOptionId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    ScheduledTestId = table.Column<long>(type: "bigint", nullable: false),
                    CandidateId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChoiceAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChoiceAnswers_AnswerOptions_AnswerOptionId",
                        column: x => x.AnswerOptionId,
                        principalTable: "AnswerOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChoiceAnswers_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChoiceAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChoiceAnswers_ScheduledTests_ScheduledTestId",
                        column: x => x.ScheduledTestId,
                        principalTable: "ScheduledTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChoiceMarks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mark = table.Column<double>(type: "float", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    TestInstanceId = table.Column<long>(type: "bigint", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChoiceMarks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EssayMarks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<float>(type: "real", nullable: false),
                    Grammar = table.Column<float>(type: "real", nullable: false),
                    Vocabulary = table.Column<float>(type: "real", nullable: false),
                    Mechanics = table.Column<float>(type: "real", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    TestInstanceId = table.Column<long>(type: "bigint", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EssayMarks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledTestInstances",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidateId = table.Column<long>(type: "bigint", nullable: false),
                    ScheduledTestId = table.Column<long>(type: "bigint", nullable: false),
                    Keycode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledTestInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledTestInstances_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduledTestInstances_ScheduledTests_ScheduledTestId",
                        column: x => x.ScheduledTestId,
                        principalTable: "ScheduledTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextAnswers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    ScheduledTestId = table.Column<long>(type: "bigint", nullable: false),
                    CandidateId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextAnswers_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TextAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TextAnswers_ScheduledTests_ScheduledTestId",
                        column: x => x.ScheduledTestId,
                        principalTable: "ScheduledTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChoiceAnswers_AnswerOptionId",
                table: "ChoiceAnswers",
                column: "AnswerOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChoiceAnswers_CandidateId",
                table: "ChoiceAnswers",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_ChoiceAnswers_QuestionId",
                table: "ChoiceAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChoiceAnswers_ScheduledTestId",
                table: "ChoiceAnswers",
                column: "ScheduledTestId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledTestInstances_CandidateId",
                table: "ScheduledTestInstances",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledTestInstances_ScheduledTestId",
                table: "ScheduledTestInstances",
                column: "ScheduledTestId");

            migrationBuilder.CreateIndex(
                name: "IX_TextAnswers_CandidateId",
                table: "TextAnswers",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_TextAnswers_QuestionId",
                table: "TextAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TextAnswers_ScheduledTestId",
                table: "TextAnswers",
                column: "ScheduledTestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChoiceAnswers");

            migrationBuilder.DropTable(
                name: "ChoiceMarks");

            migrationBuilder.DropTable(
                name: "EssayMarks");

            migrationBuilder.DropTable(
                name: "ScheduledTestInstances");

            migrationBuilder.DropTable(
                name: "TextAnswers");

            migrationBuilder.AddColumn<string>(
                name: "Keycode",
                table: "ScheduledTests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pin",
                table: "ScheduledTests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Status",
                table: "ScheduledTests",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

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
    }
}
