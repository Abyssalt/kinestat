using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class changementAttribut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pathology_Redflag_RedFlagId",
                table: "Pathology");

            migrationBuilder.DropForeignKey(
                name: "FK_Redflag_Category_CategoryId",
                table: "Redflag");


            migrationBuilder.RenameColumn(
                name: "RedFlagId",
                table: "Pathology",
                newName: "RedflagId");

            migrationBuilder.RenameIndex(
                name: "IX_Pathology_RedFlagId",
                table: "Pathology",
                newName: "IX_Pathology_RedflagId");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Redflag",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "CategoryPathology",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "integer", nullable: false),
                    PathologiesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPathology", x => new { x.CategoriesId, x.PathologiesId });
                    table.ForeignKey(
                        name: "FK_CategoryPathology_Category_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryPathology_Pathology_PathologiesId",
                        column: x => x.PathologiesId,
                        principalTable: "Pathology",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPathology_PathologiesId",
                table: "CategoryPathology",
                column: "PathologiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pathology_Redflag_RedflagId",
                table: "Pathology",
                column: "RedflagId",
                principalTable: "Redflag",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Redflag_Category_CategoryId",
                table: "Redflag",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pathology_Redflag_RedflagId",
                table: "Pathology");

            migrationBuilder.DropForeignKey(
                name: "FK_Redflag_Category_CategoryId",
                table: "Redflag");

            migrationBuilder.DropTable(
                name: "CategoryPathology");

            migrationBuilder.RenameColumn(
                name: "RedflagId",
                table: "Pathology",
                newName: "RedFlagId");

            migrationBuilder.RenameIndex(
                name: "IX_Pathology_RedflagId",
                table: "Pathology",
                newName: "IX_Pathology_RedFlagId");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Redflag",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategorieId",
                table: "Pathology",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Pathology",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pathology_CategoryId",
                table: "Pathology",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pathology_Category_CategoryId",
                table: "Pathology",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pathology_Redflag_RedFlagId",
                table: "Pathology",
                column: "RedFlagId",
                principalTable: "Redflag",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Redflag_Category_CategoryId",
                table: "Redflag",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
