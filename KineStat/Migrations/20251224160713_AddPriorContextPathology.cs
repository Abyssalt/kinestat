using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class AddPriorContextPathology : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriorContextPathologies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    PathologyId = table.Column<int>(type: "integer", nullable: false),
                    MedicalContextId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriorContextPathologies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriorContextPathologies_MedicalContexts_MedicalContextId",
                        column: x => x.MedicalContextId,
                        principalTable: "MedicalContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriorContextPathologies_Pathologies_PathologyId",
                        column: x => x.PathologyId,
                        principalTable: "Pathologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriorContextPathologies_MedicalContextId",
                table: "PriorContextPathologies",
                column: "MedicalContextId");

            migrationBuilder.CreateIndex(
                name: "IX_PriorContextPathologies_PathologyId",
                table: "PriorContextPathologies",
                column: "PathologyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriorContextPathologies");
        }
    }
}
