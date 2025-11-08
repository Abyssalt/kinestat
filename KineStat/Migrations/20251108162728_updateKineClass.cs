using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class updateKineClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FistName",
                table: "Kine",
                newName: "Password");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Kine",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "INAMINumber",
                table: "Kine",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Kine");

            migrationBuilder.DropColumn(
                name: "INAMINumber",
                table: "Kine");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Kine",
                newName: "FistName");
        }
    }
}
