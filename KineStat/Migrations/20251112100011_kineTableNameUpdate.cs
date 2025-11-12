using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class kineTableNameUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bilan_Kine_PhysioId",
                table: "Bilan");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Kine_PhysioId",
                table: "Patients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kine",
                table: "Kine");

            migrationBuilder.RenameTable(
                name: "Kine",
                newName: "Physio");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Physio",
                table: "Physio",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bilan_Physio_PhysioId",
                table: "Bilan",
                column: "PhysioId",
                principalTable: "Physio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Physio_PhysioId",
                table: "Patients",
                column: "PhysioId",
                principalTable: "Physio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bilan_Physio_PhysioId",
                table: "Bilan");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Physio_PhysioId",
                table: "Patients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Physio",
                table: "Physio");

            migrationBuilder.RenameTable(
                name: "Physio",
                newName: "Kine");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kine",
                table: "Kine",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bilan_Kine_PhysioId",
                table: "Bilan",
                column: "PhysioId",
                principalTable: "Kine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Kine_PhysioId",
                table: "Patients",
                column: "PhysioId",
                principalTable: "Kine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
