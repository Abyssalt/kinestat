using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientAnswerTests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Cluster",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PatientAnswerTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: true),
                    AnswerId = table.Column<int>(type: "integer", nullable: true),
                    DateResponse = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResponseValue = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Observations = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsCustomTest = table.Column<bool>(type: "boolean", nullable: false),
                    CustomTestName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CustomTestType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAnswerTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAnswerTests_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientAnswerTests_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAnswerTests_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerTests_AnswerId",
                table: "PatientAnswerTests",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerTests_PatientId",
                table: "PatientAnswerTests",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerTests_QuestionId",
                table: "PatientAnswerTests",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientAnswerTests");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Cluster");
        }
    }
}
