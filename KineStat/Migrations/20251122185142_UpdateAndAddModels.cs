using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAndAddModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cluster_Category_CategoryId",
                table: "Cluster");

            migrationBuilder.DropForeignKey(
                name: "FK_Pathology_Category_CategoryId",
                table: "Pathology");

            migrationBuilder.DropForeignKey(
                name: "FK_Pathology_Redflag_RedFlagId",
                table: "Pathology");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Physio_PhysioId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Bilan_BilanID",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Pathology_PathologyId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Redflag_RedflagId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Redflag_Bilan_BilanId",
                table: "Redflag");

            migrationBuilder.DropForeignKey(
                name: "FK_Redflag_Category_CategoryId",
                table: "Redflag");

            migrationBuilder.DropTable(
                name: "AnswerBool");

            migrationBuilder.DropTable(
                name: "AnswerLadder");

            migrationBuilder.DropTable(
                name: "AnswerQCM");

            migrationBuilder.DropTable(
                name: "Bilan");

            migrationBuilder.DropIndex(
                name: "IX_Question_PathologyId",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Redflag",
                table: "Redflag");

            migrationBuilder.DropIndex(
                name: "IX_Redflag_BilanId",
                table: "Redflag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Physio",
                table: "Physio");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pathology",
                table: "Pathology");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ListAnswers",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "PathologyId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "BilanId",
                table: "Redflag");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Redflag");

            migrationBuilder.RenameTable(
                name: "Redflag",
                newName: "Redflags");

            migrationBuilder.RenameTable(
                name: "Physio",
                newName: "Physios");

            migrationBuilder.RenameTable(
                name: "Pathology",
                newName: "Pathologies");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameColumn(
                name: "min",
                table: "Question",
                newName: "Min");

            migrationBuilder.RenameColumn(
                name: "max",
                table: "Question",
                newName: "Max");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Question",
                newName: "SourceRv");

            migrationBuilder.RenameColumn(
                name: "RedflagId",
                table: "Question",
                newName: "AssessmentId");

            migrationBuilder.RenameColumn(
                name: "RVPositif",
                table: "Question",
                newName: "RVPositive");

            migrationBuilder.RenameColumn(
                name: "RVNegatif",
                table: "Question",
                newName: "RVNegative");

            migrationBuilder.RenameColumn(
                name: "BilanID",
                table: "Question",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Question_RedflagId",
                table: "Question",
                newName: "IX_Question_AssessmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Question_BilanID",
                table: "Question",
                newName: "IX_Question_CategoryId");

            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Patients",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "SeverityLevel",
                table: "Redflags",
                newName: "PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Redflag_CategoryId",
                table: "Redflags",
                newName: "IX_Redflags_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Pathology_RedFlagId",
                table: "Pathologies",
                newName: "IX_Pathologies_RedFlagId");

            migrationBuilder.RenameIndex(
                name: "IX_Pathology_CategoryId",
                table: "Pathologies",
                newName: "IX_Pathologies_CategoryId");

            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "Categories",
                newName: "Name");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Patients",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Redflags",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssessmentId",
                table: "Redflags",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "Redflags",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Prior",
                table: "Categories",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Redflags",
                table: "Redflags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Physios",
                table: "Physios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pathologies",
                table: "Pathologies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    QuestionQCMId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answers_Question_QuestionQCMId",
                        column: x => x.QuestionQCMId,
                        principalTable: "Question",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClinicalDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicalDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicalDatas_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClinicalDatas_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalContexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalContexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    ArchivedAt = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PathologyQuestion",
                columns: table => new
                {
                    PathologiesId = table.Column<int>(type: "integer", nullable: false),
                    QuestionsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PathologyQuestion", x => new { x.PathologiesId, x.QuestionsId });
                    table.ForeignKey(
                        name: "FK_PathologyQuestion_Pathologies_PathologiesId",
                        column: x => x.PathologiesId,
                        principalTable: "Pathologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PathologyQuestion_Question_QuestionsId",
                        column: x => x.QuestionsId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RedflagThresholds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ThresholdPercentage = table.Column<double>(type: "double precision", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedflagThresholds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Socrates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssessmentId = table.Column<int>(type: "integer", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    Site = table.Column<string>(type: "text", nullable: false),
                    Onset = table.Column<string>(type: "text", nullable: false),
                    Character = table.Column<string>(type: "text", nullable: false),
                    Radiation = table.Column<string>(type: "text", nullable: false),
                    Association = table.Column<string>(type: "text", nullable: false),
                    Timing = table.Column<string>(type: "text", nullable: false),
                    ExacerbatingFactor = table.Column<string>(type: "text", nullable: false),
                    RelievingFactor = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socrates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Socrates_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriorContexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    MedicalContextId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriorContexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriorContexts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriorContexts_MedicalContexts_MedicalContextId",
                        column: x => x.MedicalContextId,
                        principalTable: "MedicalContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    PhysioId = table.Column<int>(type: "integer", nullable: false),
                    MedicalRecordId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assessments_MedicalRecords_MedicalRecordId",
                        column: x => x.MedicalRecordId,
                        principalTable: "MedicalRecords",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Assessments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assessments_Physios_PhysioId",
                        column: x => x.PhysioId,
                        principalTable: "Physios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientAnswerBools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<bool>(type: "boolean", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    AssessmentId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAnswerBools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAnswerBools_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAnswerBools_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAnswerBools_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientAnswerNumerics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    AssessmentId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAnswerNumerics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAnswerNumerics_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAnswerNumerics_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAnswerNumerics_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientAnswerQCMs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnswerId = table.Column<int>(type: "integer", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    AssessmentId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAnswerQCMs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAnswerQCMs_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAnswerQCMs_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAnswerQCMs_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAnswerQCMs_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Redflags_AssessmentId",
                table: "Redflags",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Redflags_PatientId",
                table: "Redflags",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionQCMId",
                table: "Answers",
                column: "QuestionQCMId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_MedicalRecordId",
                table: "Assessments",
                column: "MedicalRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_PatientId",
                table: "Assessments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_PhysioId",
                table: "Assessments",
                column: "PhysioId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalDatas_CategoryId",
                table: "ClinicalDatas",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalDatas_PatientId",
                table: "ClinicalDatas",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PathologyQuestion_QuestionsId",
                table: "PathologyQuestion",
                column: "QuestionsId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerBools_AssessmentId",
                table: "PatientAnswerBools",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerBools_PatientId",
                table: "PatientAnswerBools",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerBools_QuestionId",
                table: "PatientAnswerBools",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerNumerics_AssessmentId",
                table: "PatientAnswerNumerics",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerNumerics_PatientId",
                table: "PatientAnswerNumerics",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerNumerics_QuestionId",
                table: "PatientAnswerNumerics",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerQCMs_AnswerId",
                table: "PatientAnswerQCMs",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerQCMs_AssessmentId",
                table: "PatientAnswerQCMs",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerQCMs_PatientId",
                table: "PatientAnswerQCMs",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerQCMs_QuestionId",
                table: "PatientAnswerQCMs",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_PriorContexts_CategoryId",
                table: "PriorContexts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PriorContexts_MedicalContextId",
                table: "PriorContexts",
                column: "MedicalContextId");

            migrationBuilder.CreateIndex(
                name: "IX_Socrates_PatientId",
                table: "Socrates",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cluster_Categories_CategoryId",
                table: "Cluster",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pathologies_Categories_CategoryId",
                table: "Pathologies",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pathologies_Redflags_RedFlagId",
                table: "Pathologies",
                column: "RedFlagId",
                principalTable: "Redflags",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Physios_PhysioId",
                table: "Patients",
                column: "PhysioId",
                principalTable: "Physios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Assessments_AssessmentId",
                table: "Question",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Categories_CategoryId",
                table: "Question",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Redflags_Assessments_AssessmentId",
                table: "Redflags",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Redflags_Categories_CategoryId",
                table: "Redflags",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Redflags_Patients_PatientId",
                table: "Redflags",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cluster_Categories_CategoryId",
                table: "Cluster");

            migrationBuilder.DropForeignKey(
                name: "FK_Pathologies_Categories_CategoryId",
                table: "Pathologies");

            migrationBuilder.DropForeignKey(
                name: "FK_Pathologies_Redflags_RedFlagId",
                table: "Pathologies");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Physios_PhysioId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Assessments_AssessmentId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Categories_CategoryId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Redflags_Assessments_AssessmentId",
                table: "Redflags");

            migrationBuilder.DropForeignKey(
                name: "FK_Redflags_Categories_CategoryId",
                table: "Redflags");

            migrationBuilder.DropForeignKey(
                name: "FK_Redflags_Patients_PatientId",
                table: "Redflags");

            migrationBuilder.DropTable(
                name: "ClinicalDatas");

            migrationBuilder.DropTable(
                name: "PathologyQuestion");

            migrationBuilder.DropTable(
                name: "PatientAnswerBools");

            migrationBuilder.DropTable(
                name: "PatientAnswerNumerics");

            migrationBuilder.DropTable(
                name: "PatientAnswerQCMs");

            migrationBuilder.DropTable(
                name: "PriorContexts");

            migrationBuilder.DropTable(
                name: "RedflagThresholds");

            migrationBuilder.DropTable(
                name: "Socrates");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropTable(
                name: "MedicalContexts");

            migrationBuilder.DropTable(
                name: "MedicalRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Redflags",
                table: "Redflags");

            migrationBuilder.DropIndex(
                name: "IX_Redflags_AssessmentId",
                table: "Redflags");

            migrationBuilder.DropIndex(
                name: "IX_Redflags_PatientId",
                table: "Redflags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Physios",
                table: "Physios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pathologies",
                table: "Pathologies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "AssessmentId",
                table: "Redflags");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Redflags");

            migrationBuilder.DropColumn(
                name: "Prior",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Redflags",
                newName: "Redflag");

            migrationBuilder.RenameTable(
                name: "Physios",
                newName: "Physio");

            migrationBuilder.RenameTable(
                name: "Pathologies",
                newName: "Pathology");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "Min",
                table: "Question",
                newName: "min");

            migrationBuilder.RenameColumn(
                name: "Max",
                table: "Question",
                newName: "max");

            migrationBuilder.RenameColumn(
                name: "SourceRv",
                table: "Question",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "RVPositive",
                table: "Question",
                newName: "RVPositif");

            migrationBuilder.RenameColumn(
                name: "RVNegative",
                table: "Question",
                newName: "RVNegatif");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Question",
                newName: "BilanID");

            migrationBuilder.RenameColumn(
                name: "AssessmentId",
                table: "Question",
                newName: "RedflagId");

            migrationBuilder.RenameIndex(
                name: "IX_Question_CategoryId",
                table: "Question",
                newName: "IX_Question_BilanID");

            migrationBuilder.RenameIndex(
                name: "IX_Question_AssessmentId",
                table: "Question",
                newName: "IX_Question_RedflagId");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "Patients",
                newName: "Genre");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Redflag",
                newName: "SeverityLevel");

            migrationBuilder.RenameIndex(
                name: "IX_Redflags_CategoryId",
                table: "Redflag",
                newName: "IX_Redflag_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Pathologies_RedFlagId",
                table: "Pathology",
                newName: "IX_Pathology_RedFlagId");

            migrationBuilder.RenameIndex(
                name: "IX_Pathologies_CategoryId",
                table: "Pathology",
                newName: "IX_Pathology_CategoryId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Category",
                newName: "Nom");

            migrationBuilder.AddColumn<bool>(
                name: "Answer",
                table: "Question",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "ListAnswers",
                table: "Question",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PathologyId",
                table: "Question",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Redflag",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "BilanId",
                table: "Redflag",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Redflag",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Redflag",
                table: "Redflag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Physio",
                table: "Physio",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pathology",
                table: "Pathology",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AnswerBool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    BoolValue = table.Column<bool>(type: "boolean", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
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
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    LadderValue = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
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
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    SelectedAnswers = table.Column<List<string>>(type: "text[]", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Bilan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    PhysioId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Question_PathologyId",
                table: "Question",
                column: "PathologyId");

            migrationBuilder.CreateIndex(
                name: "IX_Redflag_BilanId",
                table: "Redflag",
                column: "BilanId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Cluster_Category_CategoryId",
                table: "Cluster",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");

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
                name: "FK_Patients_Physio_PhysioId",
                table: "Patients",
                column: "PhysioId",
                principalTable: "Physio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Bilan_BilanID",
                table: "Question",
                column: "BilanID",
                principalTable: "Bilan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Pathology_PathologyId",
                table: "Question",
                column: "PathologyId",
                principalTable: "Pathology",
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
                principalColumn: "Id");
        }
    }
}
