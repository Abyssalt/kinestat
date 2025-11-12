using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class modelsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bilan_Kine_KineId",
                table: "Bilan");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Kine_KineId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Redflags_Bilan_BilanId",
                table: "Redflags");

            migrationBuilder.DropTable(
                name: "QuestionBool");

            migrationBuilder.DropTable(
                name: "QuestionLadder");

            migrationBuilder.DropTable(
                name: "QuestionQCM");

            migrationBuilder.DropIndex(
                name: "IX_Redflags_BilanId",
                table: "Redflags");

            migrationBuilder.DropIndex(
                name: "IX_Patients_KineId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "BilanId",
                table: "Redflags");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Redflags");

            migrationBuilder.DropColumn(
                name: "KineId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Cluster");

            migrationBuilder.RenameColumn(
                name: "KineId",
                table: "Bilan",
                newName: "PhysioId");

            migrationBuilder.RenameIndex(
                name: "IX_Bilan_KineId",
                table: "Bilan",
                newName: "IX_Bilan_PhysioId");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Redflags",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeverityLevel",
                table: "Redflags",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Answer",
                table: "Question",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BilanID",
                table: "Question",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Question",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<List<string>>(
                name: "ListAnswers",
                table: "Question",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "max",
                table: "Question",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "min",
                table: "Question",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhysioId",
                table: "Patients",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Cluster",
                type: "integer",
                nullable: true);

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

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Prior = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pathology",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Prior = table.Column<double>(type: "double precision", nullable: false),
                    CategorieId = table.Column<int>(type: "integer", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    RedFlagId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pathology", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pathology_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pathology_Redflags_RedFlagId",
                        column: x => x.RedFlagId,
                        principalTable: "Redflags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Question_BilanID",
                table: "Question",
                column: "BilanID");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PhysioId",
                table: "Patients",
                column: "PhysioId");

            migrationBuilder.CreateIndex(
                name: "IX_Cluster_CategoryId",
                table: "Cluster",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_BilanRedflag_RedFlagsDetectedId",
                table: "BilanRedflag",
                column: "RedFlagsDetectedId");

            migrationBuilder.CreateIndex(
                name: "IX_Pathology_CategoryId",
                table: "Pathology",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Pathology_RedFlagId",
                table: "Pathology",
                column: "RedFlagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bilan_Kine_PhysioId",
                table: "Bilan",
                column: "PhysioId",
                principalTable: "Kine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cluster_Category_CategoryId",
                table: "Cluster",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Kine_PhysioId",
                table: "Patients",
                column: "PhysioId",
                principalTable: "Kine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Bilan_BilanID",
                table: "Question",
                column: "BilanID",
                principalTable: "Bilan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bilan_Kine_PhysioId",
                table: "Bilan");

            migrationBuilder.DropForeignKey(
                name: "FK_Cluster_Category_CategoryId",
                table: "Cluster");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Kine_PhysioId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Bilan_BilanID",
                table: "Question");

            migrationBuilder.DropTable(
                name: "BilanRedflag");

            migrationBuilder.DropTable(
                name: "Pathology");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Question_BilanID",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Patients_PhysioId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Cluster_CategoryId",
                table: "Cluster");

            migrationBuilder.DropIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Redflags");

            migrationBuilder.DropColumn(
                name: "SeverityLevel",
                table: "Redflags");

            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "BilanID",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ListAnswers",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "max",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "min",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "PhysioId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Cluster");

            migrationBuilder.RenameColumn(
                name: "PhysioId",
                table: "Bilan",
                newName: "KineId");

            migrationBuilder.RenameIndex(
                name: "IX_Bilan_PhysioId",
                table: "Bilan",
                newName: "IX_Bilan_KineId");

            migrationBuilder.AddColumn<int>(
                name: "BilanId",
                table: "Redflags",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Redflags",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "KineId",
                table: "Patients",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Cluster",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "QuestionBool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionBool", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionBool_Question_Id",
                        column: x => x.Id,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionLadder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    max = table.Column<int>(type: "integer", nullable: false),
                    min = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionLadder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionLadder_Question_Id",
                        column: x => x.Id,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionQCM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ListAnswers = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionQCM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionQCM_Question_Id",
                        column: x => x.Id,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Redflags_BilanId",
                table: "Redflags",
                column: "BilanId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_KineId",
                table: "Patients",
                column: "KineId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer",
                column: "QuestionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bilan_Kine_KineId",
                table: "Bilan",
                column: "KineId",
                principalTable: "Kine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Kine_KineId",
                table: "Patients",
                column: "KineId",
                principalTable: "Kine",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Redflags_Bilan_BilanId",
                table: "Redflags",
                column: "BilanId",
                principalTable: "Bilan",
                principalColumn: "Id");
        }
    }
}
