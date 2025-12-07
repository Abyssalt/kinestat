using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class AddAssessmentIdToClinicalData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssessmentId",
                table: "ClinicalDatas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalDatas_AssessmentId",
                table: "ClinicalDatas",
                column: "AssessmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicalDatas_Assessments_AssessmentId",
                table: "ClinicalDatas",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClinicalDatas_Assessments_AssessmentId",
                table: "ClinicalDatas");

            migrationBuilder.DropIndex(
                name: "IX_ClinicalDatas_AssessmentId",
                table: "ClinicalDatas");

            migrationBuilder.DropColumn(
                name: "AssessmentId",
                table: "ClinicalDatas");
        }
    }
}
