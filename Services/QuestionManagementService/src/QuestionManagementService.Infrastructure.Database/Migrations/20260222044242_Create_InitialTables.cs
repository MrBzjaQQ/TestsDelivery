using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestionManagementService.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Create_InitialTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "question_banks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_banks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "test_templates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    default_duration = table.Column<int>(type: "integer", nullable: false),
                    default_passing_score = table.Column<byte>(type: "smallint", nullable: false),
                    configuration = table.Column<string>(type: "JSONB", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_templates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    text = table.Column<string>(type: "TEXT", nullable: false),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    difficulty = table.Column<byte>(type: "smallint", nullable: false),
                    question_bank_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.id);
                    table.ForeignKey(
                        name: "fk_questions_question_banks",
                        column: x => x.question_bank_id,
                        principalTable: "question_banks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    question_bank_id = table.Column<Guid>(type: "uuid", nullable: false),
                    template_id = table.Column<Guid>(type: "uuid", nullable: true),
                    duration_minutes = table.Column<int>(type: "integer", nullable: false),
                    passing_score = table.Column<byte>(type: "smallint", nullable: false),
                    max_attempts = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Draft"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tests", x => x.id);
                    table.ForeignKey(
                        name: "fk_tests_question_banks",
                        column: x => x.question_bank_id,
                        principalTable: "question_banks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tests_test_templates",
                        column: x => x.template_id,
                        principalTable: "test_templates",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "answer_options",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    question_id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_correct = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ordinal = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answer_options", x => x.id);
                    table.ForeignKey(
                        name: "fk_answer_options_questions",
                        column: x => x.question_id,
                        principalTable: "questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "test_questions",
                columns: table => new
                {
                    test_id = table.Column<Guid>(type: "uuid", nullable: false),
                    question_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ordinal = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_questions", x => new { x.test_id, x.question_id });
                    table.ForeignKey(
                        name: "fk_test_questions_questions",
                        column: x => x.question_id,
                        principalTable: "questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_test_questions_tests",
                        column: x => x.test_id,
                        principalTable: "tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_answer_options_ordinal",
                table: "answer_options",
                column: "ordinal");

            migrationBuilder.CreateIndex(
                name: "ix_answer_options_question_id",
                table: "answer_options",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "ix_question_banks_name",
                table: "question_banks",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_question_banks_owner_id",
                table: "question_banks",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_questions_category",
                table: "questions",
                column: "category");

            migrationBuilder.CreateIndex(
                name: "ix_questions_difficulty",
                table: "questions",
                column: "difficulty");

            migrationBuilder.CreateIndex(
                name: "ix_questions_question_bank_id",
                table: "questions",
                column: "question_bank_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_questions_question_id",
                table: "test_questions",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "ix_test_templates_name",
                table: "test_templates",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_tests_question_bank_id",
                table: "tests",
                column: "question_bank_id");

            migrationBuilder.CreateIndex(
                name: "ix_tests_status",
                table: "tests",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_tests_template_id",
                table: "tests",
                column: "template_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answer_options");

            migrationBuilder.DropTable(
                name: "test_questions");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "tests");

            migrationBuilder.DropTable(
                name: "question_banks");

            migrationBuilder.DropTable(
                name: "test_templates");
        }
    }
}
