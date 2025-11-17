using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class FixTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pathology_Redflags_RedFlagId",
                table: "Pathology");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Redflags_RedflagId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Redflags_Bilan_BilanId",
                table: "Redflags");

            migrationBuilder.DropForeignKey(
                name: "FK_Redflags_Category_CategoryId",
                table: "Redflags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Redflags",
                table: "Redflags");

            migrationBuilder.RenameTable(
                name: "Redflags",
                newName: "Redflag");

            migrationBuilder.RenameIndex(
                name: "IX_Redflags_CategoryId",
                table: "Redflag",
                newName: "IX_Redflag_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Redflags_BilanId",
                table: "Redflag",
                newName: "IX_Redflag_BilanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Redflag",
                table: "Redflag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pathology_Redflag_RedFlagId",
                table: "Pathology",
                column: "RedFlagId",
                principalTable: "Redflag",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Redflag_RedflagId",
                table: "Question",
                column: "RedflagId",
                principalTable: "Redflag",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Redflag_Bilan_BilanId",
                table: "Redflag",
                column: "BilanId",
                principalTable: "Bilan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Redflag_Category_CategoryId",
                table: "Redflag",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pathology_Redflag_RedFlagId",
                table: "Pathology");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Redflag_RedflagId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Redflag_Bilan_BilanId",
                table: "Redflag");

            migrationBuilder.DropForeignKey(
                name: "FK_Redflag_Category_CategoryId",
                table: "Redflag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Redflag",
                table: "Redflag");

            migrationBuilder.RenameTable(
                name: "Redflag",
                newName: "Redflags");

            migrationBuilder.RenameIndex(
                name: "IX_Redflag_CategoryId",
                table: "Redflags",
                newName: "IX_Redflags_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Redflag_BilanId",
                table: "Redflags",
                newName: "IX_Redflags_BilanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Redflags",
                table: "Redflags",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pathology_Redflags_RedFlagId",
                table: "Pathology",
                column: "RedFlagId",
                principalTable: "Redflags",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Redflags_RedflagId",
                table: "Question",
                column: "RedflagId",
                principalTable: "Redflags",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Redflags_Bilan_BilanId",
                table: "Redflags",
                column: "BilanId",
                principalTable: "Bilan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Redflags_Category_CategoryId",
                table: "Redflags",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
