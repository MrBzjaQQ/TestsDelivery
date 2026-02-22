using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using StudentManagementService.Domain.Entities;

#nullable disable

namespace StudentManagementService.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Create_StudentTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "study_groups",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    start_year = table.Column<short>(type: "smallint", nullable: false),
                    end_year = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_study_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    group_id = table.Column<Guid>(type: "uuid", nullable: true),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    profile = table.Column<StudentProfile>(type: "jsonb", nullable: true),
                    status = table.Column<int>(type: "integer", maxLength: 20, nullable: false, defaultValue: 0),
                    enrollment_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.id);
                    table.ForeignKey(
                        name: "FK_students_study_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "study_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "test_assignments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    test_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assigned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<int>(type: "integer", maxLength: 20, nullable: false, defaultValue: 0),
                    attempts_allowed = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)3),
                    attempts_used = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_assignments", x => x.id);
                    table.ForeignKey(
                        name: "FK_test_assignments_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "test_progress",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    test_id = table.Column<Guid>(type: "uuid", nullable: false),
                    attempt_number = table.Column<short>(type: "smallint", nullable: false),
                    score = table.Column<short>(type: "smallint", nullable: true),
                    max_score = table.Column<short>(type: "smallint", nullable: false),
                    is_passed = table.Column<bool>(type: "boolean", nullable: false),
                    submitted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    answers = table.Column<List<Answer>>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_progress", x => x.id);
                    table.UniqueConstraint("AK_test_progress_student_id_test_id_attempt_number", x => new { x.student_id, x.test_id, x.attempt_number });
                    table.ForeignKey(
                        name: "FK_test_progress_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_students_group_id",
                table: "students",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "ix_students_status",
                table: "students",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_students_user_id",
                table: "students",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_test_assignments_status",
                table: "test_assignments",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_test_assignments_student_id",
                table: "test_assignments",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "ix_test_assignments_test_id",
                table: "test_assignments",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "ix_test_progress_student_id",
                table: "test_progress",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "ix_test_progress_test_id",
                table: "test_progress",
                column: "test_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "test_assignments");

            migrationBuilder.DropTable(
                name: "test_progress");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "study_groups");
        }
    }
}
