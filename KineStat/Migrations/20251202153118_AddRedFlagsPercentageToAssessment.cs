using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class AddRedFlagsPercentageToAssessment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RedFlagsPercentage",
                table: "Assessments",
                type: "double precision",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Socrates_AssessmentId",
                table: "Socrates",
                column: "AssessmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Socrates_Assessments_AssessmentId",
                table: "Socrates",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Socrates_Assessments_AssessmentId",
                table: "Socrates");

            migrationBuilder.DropIndex(
                name: "IX_Socrates_AssessmentId",
                table: "Socrates");

            migrationBuilder.DropColumn(
                name: "RedFlagsPercentage",
                table: "Assessments");
        }
    }
}
