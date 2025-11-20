using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nom = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Physio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    INAMINumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Physio", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cluster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cluster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cluster_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    Height = table.Column<double>(type: "double precision", nullable: false),
                    Genre = table.Column<int>(type: "integer", nullable: false),
                    SocialSecurityNumber = table.Column<int>(type: "integer", nullable: false),
                    DoctorName = table.Column<string>(type: "text", nullable: true),
                    DoctorINAMI = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    PhysioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_Physio_PhysioId",
                        column: x => x.PhysioId,
                        principalTable: "Physio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bilan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    PhysioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bilan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bilan_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bilan_Physio_PhysioId",
                        column: x => x.PhysioId,
                        principalTable: "Physio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Redflag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    SeverityLevel = table.Column<int>(type: "integer", nullable: false),
                    BilanId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Redflag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Redflag_Bilan_BilanId",
                        column: x => x.BilanId,
                        principalTable: "Bilan",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Redflag_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pathology",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Prior = table.Column<double>(type: "double precision", nullable: false),
                    RedFlagId = table.Column<int>(type: "integer", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: true)
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
                        name: "FK_Pathology_Redflag_RedFlagId",
                        column: x => x.RedFlagId,
                        principalTable: "Redflag",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    RVPositif = table.Column<double>(type: "double precision", nullable: false),
                    RVNegatif = table.Column<double>(type: "double precision", nullable: false),
                    ClusterId = table.Column<int>(type: "integer", nullable: true),
                    BilanID = table.Column<int>(type: "integer", nullable: false),
                    RedflagId = table.Column<int>(type: "integer", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    PathologyId = table.Column<int>(type: "integer", nullable: true),
                    Answer = table.Column<bool>(type: "boolean", nullable: true),
                    min = table.Column<int>(type: "integer", nullable: true),
                    max = table.Column<int>(type: "integer", nullable: true),
                    ListAnswers = table.Column<List<string>>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_Bilan_BilanID",
                        column: x => x.BilanID,
                        principalTable: "Bilan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Question_Cluster_ClusterId",
                        column: x => x.ClusterId,
                        principalTable: "Cluster",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Question_Pathology_PathologyId",
                        column: x => x.PathologyId,
                        principalTable: "Pathology",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Question_Redflag_RedflagId",
                        column: x => x.RedflagId,
                        principalTable: "Redflag",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnswerBool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BoolValue = table.Column<bool>(type: "boolean", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerBool", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerBool_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerLadder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LadderValue = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerLadder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerLadder_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerQCM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SelectedAnswers = table.Column<List<string>>(type: "text[]", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerQCM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerQCM_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerBool_QuestionId",
                table: "AnswerBool",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerLadder_QuestionId",
                table: "AnswerLadder",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerQCM_QuestionId",
                table: "AnswerQCM",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bilan_PatientId",
                table: "Bilan",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Bilan_PhysioId",
                table: "Bilan",
                column: "PhysioId");

            migrationBuilder.CreateIndex(
                name: "IX_Cluster_CategoryId",
                table: "Cluster",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Pathology_CategoryId",
                table: "Pathology",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Pathology_RedFlagId",
                table: "Pathology",
                column: "RedFlagId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PhysioId",
                table: "Patients",
                column: "PhysioId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_BilanID",
                table: "Question",
                column: "BilanID");

            migrationBuilder.CreateIndex(
                name: "IX_Question_ClusterId",
                table: "Question",
                column: "ClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_PathologyId",
                table: "Question",
                column: "PathologyId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_RedflagId",
                table: "Question",
                column: "RedflagId");

            migrationBuilder.CreateIndex(
                name: "IX_Redflag_BilanId",
                table: "Redflag",
                column: "BilanId");

            migrationBuilder.CreateIndex(
                name: "IX_Redflag_CategoryId",
                table: "Redflag",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerBool");

            migrationBuilder.DropTable(
                name: "AnswerLadder");

            migrationBuilder.DropTable(
                name: "AnswerQCM");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Cluster");

            migrationBuilder.DropTable(
                name: "Pathology");

            migrationBuilder.DropTable(
                name: "Redflag");

            migrationBuilder.DropTable(
                name: "Bilan");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Physio");
        }
    }
}
