using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KineStat.Migrations
{
    /// <inheritdoc />
    public partial class AddAbstractModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Question_QuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Question_QuestionQCMId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_PathologyQuestion_Question_QuestionsId",
                table: "PathologyQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAnswerQCMs_Answers_AnswerId",
                table: "PatientAnswerQCMs");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAnswerQCMs_Assessments_AssessmentId",
                table: "PatientAnswerQCMs");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAnswerQCMs_Patients_PatientId",
                table: "PatientAnswerQCMs");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAnswerQCMs_Question_QuestionId",
                table: "PatientAnswerQCMs");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Assessments_AssessmentId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Categories_CategoryId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Cluster_ClusterId",
                table: "Question");

            migrationBuilder.DropTable(
                name: "PatientAnswerBools");

            migrationBuilder.DropTable(
                name: "PatientAnswerNumerics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Question",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientAnswerQCMs",
                table: "PatientAnswerQCMs");

            migrationBuilder.RenameTable(
                name: "Question",
                newName: "Questions");

            migrationBuilder.RenameTable(
                name: "PatientAnswerQCMs",
                newName: "PatientAnswers");

            migrationBuilder.RenameIndex(
                name: "IX_Question_ClusterId",
                table: "Questions",
                newName: "IX_Questions_ClusterId");

            migrationBuilder.RenameIndex(
                name: "IX_Question_CategoryId",
                table: "Questions",
                newName: "IX_Questions_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Question_AssessmentId",
                table: "Questions",
                newName: "IX_Questions_AssessmentId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientAnswerQCMs_QuestionId",
                table: "PatientAnswers",
                newName: "IX_PatientAnswers_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientAnswerQCMs_PatientId",
                table: "PatientAnswers",
                newName: "IX_PatientAnswers_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientAnswerQCMs_AssessmentId",
                table: "PatientAnswers",
                newName: "IX_PatientAnswers_AssessmentId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientAnswerQCMs_AnswerId",
                table: "PatientAnswers",
                newName: "IX_PatientAnswers_AnswerId");

            migrationBuilder.AlterColumn<int>(
                name: "AnswerId",
                table: "PatientAnswers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "PatientAnswers",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "PatientAnswerNumeric_Value",
                table: "PatientAnswers",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Value",
                table: "PatientAnswers",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientAnswers",
                table: "PatientAnswers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionQCMId",
                table: "Answers",
                column: "QuestionQCMId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PathologyQuestion_Questions_QuestionsId",
                table: "PathologyQuestion",
                column: "QuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAnswers_Answers_AnswerId",
                table: "PatientAnswers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAnswers_Assessments_AssessmentId",
                table: "PatientAnswers",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAnswers_Patients_PatientId",
                table: "PatientAnswers",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAnswers_Questions_QuestionId",
                table: "PatientAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Assessments_AssessmentId",
                table: "Questions",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Categories_CategoryId",
                table: "Questions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Cluster_ClusterId",
                table: "Questions",
                column: "ClusterId",
                principalTable: "Cluster",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionQCMId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_PathologyQuestion_Questions_QuestionsId",
                table: "PathologyQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAnswers_Answers_AnswerId",
                table: "PatientAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAnswers_Assessments_AssessmentId",
                table: "PatientAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAnswers_Patients_PatientId",
                table: "PatientAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAnswers_Questions_QuestionId",
                table: "PatientAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Assessments_AssessmentId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Categories_CategoryId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Cluster_ClusterId",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientAnswers",
                table: "PatientAnswers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "PatientAnswers");

            migrationBuilder.DropColumn(
                name: "PatientAnswerNumeric_Value",
                table: "PatientAnswers");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "PatientAnswers");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "Question");

            migrationBuilder.RenameTable(
                name: "PatientAnswers",
                newName: "PatientAnswerQCMs");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_ClusterId",
                table: "Question",
                newName: "IX_Question_ClusterId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_CategoryId",
                table: "Question",
                newName: "IX_Question_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_AssessmentId",
                table: "Question",
                newName: "IX_Question_AssessmentId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientAnswers_QuestionId",
                table: "PatientAnswerQCMs",
                newName: "IX_PatientAnswerQCMs_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientAnswers_PatientId",
                table: "PatientAnswerQCMs",
                newName: "IX_PatientAnswerQCMs_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientAnswers_AssessmentId",
                table: "PatientAnswerQCMs",
                newName: "IX_PatientAnswerQCMs_AssessmentId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientAnswers_AnswerId",
                table: "PatientAnswerQCMs",
                newName: "IX_PatientAnswerQCMs_AnswerId");

            migrationBuilder.AlterColumn<int>(
                name: "AnswerId",
                table: "PatientAnswerQCMs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Question",
                table: "Question",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientAnswerQCMs",
                table: "PatientAnswerQCMs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PatientAnswerBools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssessmentId = table.Column<int>(type: "integer", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<bool>(type: "boolean", nullable: false)
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
                    AssessmentId = table.Column<int>(type: "integer", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<double>(type: "double precision", nullable: false)
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

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Question_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Question_QuestionQCMId",
                table: "Answers",
                column: "QuestionQCMId",
                principalTable: "Question",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PathologyQuestion_Question_QuestionsId",
                table: "PathologyQuestion",
                column: "QuestionsId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAnswerQCMs_Answers_AnswerId",
                table: "PatientAnswerQCMs",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAnswerQCMs_Assessments_AssessmentId",
                table: "PatientAnswerQCMs",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAnswerQCMs_Patients_PatientId",
                table: "PatientAnswerQCMs",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAnswerQCMs_Question_QuestionId",
                table: "PatientAnswerQCMs",
                column: "QuestionId",
                principalTable: "Question",
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
                name: "FK_Question_Cluster_ClusterId",
                table: "Question",
                column: "ClusterId",
                principalTable: "Cluster",
                principalColumn: "Id");
        }
    }
}
