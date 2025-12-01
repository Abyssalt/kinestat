using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class addedMedicalInformationsToPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActivitesPhysiques",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AntecedentsMedicaux",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicationActuelle",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Profession",
                table: "Patients",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivitesPhysiques",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "AntecedentsMedicaux",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MedicationActuelle",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Profession",
                table: "Patients");
        }
    }
}
