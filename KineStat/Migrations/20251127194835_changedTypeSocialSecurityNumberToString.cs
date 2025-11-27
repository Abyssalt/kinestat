using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class changedTypeSocialSecurityNumberToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SocialSecurityNumber",
                table: "Patients",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SocialSecurityNumber",
                table: "Patients",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
