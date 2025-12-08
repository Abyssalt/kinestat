using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class AddAssessmentIdToPatientAnswerTests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssessmentId",
                table: "PatientAnswerTests",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssessmentId",
                table: "PatientAnswerTests");
        }
    }
}
