using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class LinkTestsToAssessment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssessmentId",
                table: "PatientAnswerTests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerTests_AssessmentId",
                table: "PatientAnswerTests",
                column: "AssessmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAnswerTests_Assessments_AssessmentId",
                table: "PatientAnswerTests",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAnswerTests_Assessments_AssessmentId",
                table: "PatientAnswerTests");

            migrationBuilder.DropIndex(
                name: "IX_PatientAnswerTests_AssessmentId",
                table: "PatientAnswerTests");

            migrationBuilder.DropColumn(
                name: "AssessmentId",
                table: "PatientAnswerTests");
        }
    }
}
