using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class PatientStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientStatus",
                table: "Patients",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientStatus",
                table: "Patients");
        }
    }
}
