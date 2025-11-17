using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class addRedFlagsRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RedFlagId",
                table: "Question",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Question_RedFlagId",
                table: "Question",
                column: "RedFlagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Redflags_RedFlagId",
                table: "Question",
                column: "RedFlagId",
                principalTable: "Redflags",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Redflags_RedFlagId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_RedFlagId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "RedFlagId",
                table: "Question");
        }
    }
}
