using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedicalContextId",
                table: "Assessments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_MedicalContextId",
                table: "Assessments",
                column: "MedicalContextId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_MedicalContexts_MedicalContextId",
                table: "Assessments",
                column: "MedicalContextId",
                principalTable: "MedicalContexts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_MedicalContexts_MedicalContextId",
                table: "Assessments");

            migrationBuilder.DropIndex(
                name: "IX_Assessments_MedicalContextId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "MedicalContextId",
                table: "Assessments");
        }
    }
}
