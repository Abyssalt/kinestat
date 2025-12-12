using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class removedFolderStatusAndAddedAssessmentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Dossiers");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Assessments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Assessments");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Dossiers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
