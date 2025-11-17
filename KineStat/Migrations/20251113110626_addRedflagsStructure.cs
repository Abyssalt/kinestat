using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class addRedflagsStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Redflags_RedFlagId",
                table: "Question");

            migrationBuilder.DropTable(
                name: "BilanRedflag");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Redflags",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "RedFlagId",
                table: "Question",
                newName: "RedflagId");

            migrationBuilder.RenameIndex(
                name: "IX_Question_RedFlagId",
                table: "Question",
                newName: "IX_Question_RedflagId");

            migrationBuilder.AddColumn<int>(
                name: "BilanId",
                table: "Redflags",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Redflags_BilanId",
                table: "Redflags",
                column: "BilanId");

            migrationBuilder.CreateIndex(
                name: "IX_Redflags_CategoryId",
                table: "Redflags",
                column: "CategoryId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Redflags_RedflagId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Redflags_Bilan_BilanId",
                table: "Redflags");

            migrationBuilder.DropForeignKey(
                name: "FK_Redflags_Category_CategoryId",
                table: "Redflags");

            migrationBuilder.DropIndex(
                name: "IX_Redflags_BilanId",
                table: "Redflags");

            migrationBuilder.DropIndex(
                name: "IX_Redflags_CategoryId",
                table: "Redflags");

            migrationBuilder.DropColumn(
                name: "BilanId",
                table: "Redflags");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Redflags",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "RedflagId",
                table: "Question",
                newName: "RedFlagId");

            migrationBuilder.RenameIndex(
                name: "IX_Question_RedflagId",
                table: "Question",
                newName: "IX_Question_RedFlagId");

            migrationBuilder.CreateTable(
                name: "BilanRedflag",
                columns: table => new
                {
                    BilansId = table.Column<int>(type: "integer", nullable: false),
                    RedFlagsDetectedId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BilanRedflag", x => new { x.BilansId, x.RedFlagsDetectedId });
                    table.ForeignKey(
                        name: "FK_BilanRedflag_Bilan_BilansId",
                        column: x => x.BilansId,
                        principalTable: "Bilan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BilanRedflag_Redflags_RedFlagsDetectedId",
                        column: x => x.RedFlagsDetectedId,
                        principalTable: "Redflags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BilanRedflag_RedFlagsDetectedId",
                table: "BilanRedflag",
                column: "RedFlagsDetectedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Redflags_RedFlagId",
                table: "Question",
                column: "RedFlagId",
                principalTable: "Redflags",
                principalColumn: "Id");
        }
    }
}
