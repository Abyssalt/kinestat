using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class AddAssessmentToDossier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DossierId",
                table: "Assessments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_DossierId",
                table: "Assessments",
                column: "DossierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Dossiers_DossierId",
                table: "Assessments",
                column: "DossierId",
                principalTable: "Dossiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Dossiers_DossierId",
                table: "Assessments");

            migrationBuilder.DropIndex(
                name: "IX_Assessments_DossierId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "DossierId",
                table: "Assessments");
        }
    }
}
