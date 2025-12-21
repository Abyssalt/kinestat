using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NumeroINAMI = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
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
                name: "Physios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    INAMINumber = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Physios", x => x.Id);
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
                name: "Cluster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    MinNumberOfPositiveTests = table.Column<int>(type: "integer", nullable: true),
                    RVPositive = table.Column<double>(type: "double precision", nullable: true),
                    RVNegative = table.Column<double>(type: "double precision", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cluster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cluster_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
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
                    Weight = table.Column<int>(type: "integer", nullable: true),
                    Height = table.Column<int>(type: "integer", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    SocialSecurityNumber = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<int>(type: "integer", nullable: false),
                    Profession = table.Column<string>(type: "text", nullable: true),
                    ActivitesPhysiques = table.Column<string>(type: "text", nullable: true),
                    AntecedentsMedicaux = table.Column<string>(type: "text", nullable: true),
                    MedicationActuelle = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DoctorId = table.Column<int>(type: "integer", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    PhysioId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsAnonymized = table.Column<bool>(type: "boolean", nullable: false),
                    AnonymizedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InactiveSinceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patients_Physios_PhysioId",
                        column: x => x.PhysioId,
                        principalTable: "Physios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pathologies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    ClusterId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pathologies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pathologies_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pathologies_Cluster_ClusterId",
                        column: x => x.ClusterId,
                        principalTable: "Cluster",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Dossiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titre = table.Column<string>(type: "text", nullable: false),
                    DateOuverture = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    PatientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dossiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dossiers_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientPathologiesDetecteds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    AssessmentId = table.Column<int>(type: "integer", nullable: false),
                    PathologyId = table.Column<int>(type: "integer", nullable: false),
                    PathologyProbability = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPathologiesDetecteds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientPathologiesDetecteds_Pathologies_PathologyId",
                        column: x => x.PathologyId,
                        principalTable: "Pathologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientPathologiesDetecteds_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
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
                    MedicalContextId = table.Column<int>(type: "integer", nullable: false),
                    PhysioId = table.Column<int>(type: "integer", nullable: false),
                    DossierId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RedFlagsPercentage = table.Column<double>(type: "double precision", nullable: true),
                    MedicalRecordId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assessments_Dossiers_DossierId",
                        column: x => x.DossierId,
                        principalTable: "Dossiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assessments_MedicalContexts_MedicalContextId",
                        column: x => x.MedicalContextId,
                        principalTable: "MedicalContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "ClinicalDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    AssessmentId = table.Column<int>(type: "integer", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicalDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicalDatas_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id");
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
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    RVPositive = table.Column<double>(type: "double precision", nullable: false),
                    RVNegative = table.Column<double>(type: "double precision", nullable: false),
                    SourceRv = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    HasPermanentAnswer = table.Column<bool>(type: "boolean", nullable: true),
                    ClusterId = table.Column<int>(type: "integer", nullable: true),
                    AssessmentId = table.Column<int>(type: "integer", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    Min = table.Column<int>(type: "integer", nullable: true),
                    Max = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Questions_Cluster_ClusterId",
                        column: x => x.ClusterId,
                        principalTable: "Cluster",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Redflags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    AssessmentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Redflags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Redflags_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Redflags_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Redflags_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Socrates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssessmentId = table.Column<int>(type: "integer", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    Site = table.Column<string>(type: "text", nullable: true),
                    Onset = table.Column<string>(type: "text", nullable: true),
                    Character = table.Column<string>(type: "text", nullable: true),
                    Radiation = table.Column<string>(type: "text", nullable: true),
                    Association = table.Column<string>(type: "text", nullable: true),
                    Timing = table.Column<string>(type: "text", nullable: true),
                    ExacerbatingFactor = table.Column<string>(type: "text", nullable: true),
                    RelievingFactor = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socrates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Socrates_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Socrates_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionQCMId",
                        column: x => x.QuestionQCMId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionPathologies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    PathologyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionPathologies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionPathologies_Pathologies_PathologyId",
                        column: x => x.PathologyId,
                        principalTable: "Pathologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionPathologies_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    AssessmentId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    Value = table.Column<bool>(type: "boolean", nullable: true),
                    PatientAnswerNumeric_Value = table.Column<double>(type: "double precision", nullable: true),
                    AnswerId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAnswers_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAnswers_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAnswers_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientAnswerTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: true),
                    AnswerId = table.Column<int>(type: "integer", nullable: true),
                    DateResponse = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResponseValue = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Observations = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    AssessmentId = table.Column<int>(type: "integer", nullable: true),
                    IsCustomTest = table.Column<bool>(type: "boolean", nullable: false),
                    CustomTestName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CustomTestType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAnswerTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAnswerTests_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientAnswerTests_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientAnswerTests_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAnswerTests_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionQCMId",
                table: "Answers",
                column: "QuestionQCMId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_DossierId",
                table: "Assessments",
                column: "DossierId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_MedicalContextId",
                table: "Assessments",
                column: "MedicalContextId");

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
                name: "IX_ClinicalDatas_AssessmentId",
                table: "ClinicalDatas",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalDatas_CategoryId",
                table: "ClinicalDatas",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalDatas_PatientId",
                table: "ClinicalDatas",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Cluster_CategoryId",
                table: "Cluster",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Dossiers_PatientId",
                table: "Dossiers",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Pathologies_CategoryId",
                table: "Pathologies",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Pathologies_ClusterId",
                table: "Pathologies",
                column: "ClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswers_AnswerId",
                table: "PatientAnswers",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswers_AssessmentId",
                table: "PatientAnswers",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswers_PatientId",
                table: "PatientAnswers",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswers_QuestionId",
                table: "PatientAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerTests_AnswerId",
                table: "PatientAnswerTests",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerTests_AssessmentId",
                table: "PatientAnswerTests",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerTests_PatientId",
                table: "PatientAnswerTests",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswerTests_QuestionId",
                table: "PatientAnswerTests",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPathologiesDetecteds_PathologyId",
                table: "PatientPathologiesDetecteds",
                column: "PathologyId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPathologiesDetecteds_PatientId",
                table: "PatientPathologiesDetecteds",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_DoctorId",
                table: "Patients",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PhysioId",
                table: "Patients",
                column: "PhysioId");

            migrationBuilder.CreateIndex(
                name: "IX_PriorContexts_CategoryId",
                table: "PriorContexts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PriorContexts_MedicalContextId",
                table: "PriorContexts",
                column: "MedicalContextId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionPathologies_PathologyId",
                table: "QuestionPathologies",
                column: "PathologyId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionPathologies_QuestionId_PathologyId",
                table: "QuestionPathologies",
                columns: new[] { "QuestionId", "PathologyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_AssessmentId",
                table: "Questions",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CategoryId",
                table: "Questions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ClusterId",
                table: "Questions",
                column: "ClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_Redflags_AssessmentId",
                table: "Redflags",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Redflags_CategoryId",
                table: "Redflags",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Redflags_PatientId",
                table: "Redflags",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Socrates_AssessmentId",
                table: "Socrates",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Socrates_PatientId",
                table: "Socrates",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.DropTable(
                name: "ClinicalDatas");

            migrationBuilder.DropTable(
                name: "PatientAnswers");

            migrationBuilder.DropTable(
                name: "PatientAnswerTests");

            migrationBuilder.DropTable(
                name: "PatientPathologiesDetecteds");

            migrationBuilder.DropTable(
                name: "PriorContexts");

            migrationBuilder.DropTable(
                name: "QuestionPathologies");

            migrationBuilder.DropTable(
                name: "Redflags");

            migrationBuilder.DropTable(
                name: "RedflagThresholds");

            migrationBuilder.DropTable(
                name: "Socrates");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Pathologies");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropTable(
                name: "Cluster");

            migrationBuilder.DropTable(
                name: "Dossiers");

            migrationBuilder.DropTable(
                name: "MedicalContexts");

            migrationBuilder.DropTable(
                name: "MedicalRecords");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Physios");
        }
    }
}
