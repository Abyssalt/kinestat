using KineStat.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace KineStat.Services
{
    public class AssessmentCompletePdfService
    {
        public byte[] GenerateCompletePdf(
            Patient patient,
            Socrate? socrate,
            Assessment assessment,
            List<PatientAnswerTests> tests,
            List<double> tintivValues,
            List<double> clinicalValues)
        {
            // License stuff, do not modify
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    // Header
                    page.Header()
                        .Height(60)
                        .Background(Colors.Blue.Lighten3)
                        .Padding(15)
                        .Row(row =>
                        {
                            row.RelativeItem().Column(column =>
                            {
                                column.Item().Text("Dossier Kinésithérapie")
                                    .FontSize(18)
                                    .Bold()
                                    .FontColor(Colors.Blue.Darken2);

                                column.Item().Text($"Patient : {patient.FirstName} {patient.LastName}")
                                    .FontSize(12)
                                    .FontColor(Colors.Grey.Darken2);

                                column.Item().Text($"Date : {assessment.Date:dd/MM/yyyy}")
                                    .FontSize(10)
                                    .FontColor(Colors.Grey.Darken1);
                            });
                        });

                    // Start of the content
                    page.Content()
                        .PaddingVertical(10)
                        .Column(column =>
                        {
                            // Page 1
                            AddAnamnesePage(column, patient);
                            column.Item().PageBreak();

                            // Page 2
                            if (socrate != null)
                            {
                                AddSocratePage(column, socrate);
                                column.Item().PageBreak();
                            }

                            // Page 3
                            AddResultsPage(column, tests, tintivValues, clinicalValues, assessment);
                        });

                    // Footer
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                        })
                        .FontSize(9)
                        .FontColor(Colors.Grey.Darken1);
                });
            });

            return document.GeneratePdf();
        }

        private void AddAnamnesePage(ColumnDescriptor column, Patient patient)
        {
            column.Item().Text("ANAMNÈSE")
                .FontSize(16)
                .Bold()
                .FontColor(Colors.Blue.Darken2);

            column.Item().PaddingTop(10).LineHorizontal(2).LineColor(Colors.Blue.Lighten2);

            column.Item().PaddingTop(15).Column(sectionColumn =>
            {
                sectionColumn.Item().Text("Informations administratives")
                    .FontSize(13)
                    .Bold()
                    .FontColor(Colors.Blue.Darken1);

                sectionColumn.Item().PaddingTop(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(2);
                    });

                    // Line 1
                    AddTableRow(table, "Nom", patient.LastName);
                    AddTableRow(table, "Prénom", patient.FirstName);

                    // Line 2
                    AddTableRow(table, "Genre", patient.Gender.ToString());
                    AddTableRow(table, "Date de naissance", patient.BirthDate.ToString("dd/MM/yyyy"));

                    // Line 3
                    AddTableRow(table, "Adresse", patient.Address ?? "Non renseignée");
                    AddTableRow(table, "Téléphone", patient.PhoneNumber ?? "Non renseigné");

                    // Line 4
                    AddTableRow(table, "Email", patient.Email ?? "Non renseigné");
                    AddTableRow(table, "Statut", patient.Status.ToString());

                    // Line 5
                    AddTableRow(table, "Poids (kg)", patient.Weight?.ToString() ?? "Non renseigné");
                    AddTableRow(table, "Taille (cm)", patient.Height?.ToString() ?? "Non renseignée");

                    // Line 6
                    AddTableRow(table, "N° Sécurité sociale", patient.SocialSecurityNumber ?? "Non renseigné");
                });

                // Medical information
                sectionColumn.Item().PaddingTop(20).Text("Informations médicales")
                    .FontSize(13)
                    .Bold()
                    .FontColor(Colors.Blue.Darken1);

                sectionColumn.Item().PaddingTop(10).Column(medColumn =>
                {
                    AddMedicalInfo(medColumn, "Profession", patient.Profession);
                    AddMedicalInfo(medColumn, "Activités physiques", patient.ActivitesPhysiques);
                    AddMedicalInfo(medColumn, "Antécédents médicaux", patient.AntecedentsMedicaux);
                    AddMedicalInfo(medColumn, "Médication actuelle", patient.MedicationActuelle);
                });
            });
        }

        private void AddSocratePage(ColumnDescriptor column, Socrate socrate)
        {
            column.Item().Text("QUESTIONNAIRE SOCRATE")
                .FontSize(16)
                .Bold()
                .FontColor(Colors.Blue.Darken2);

            column.Item().PaddingTop(5).Text("Évaluation de la douleur")
                .FontSize(11)
                .Italic()
                .FontColor(Colors.Grey.Darken1);

            column.Item().PaddingTop(10).LineHorizontal(2).LineColor(Colors.Blue.Lighten2);

            column.Item().PaddingTop(15).Column(sectionColumn =>
            {
                sectionColumn.Spacing(12);

                AddSocrateSection(sectionColumn, "S", "Site",
                    "Où se situe la douleur ?", socrate.Site);

                AddSocrateSection(sectionColumn, "O", "Onset (Début)",
                    "Quand et comment a débuté la douleur ?", socrate.Onset);

                AddSocrateSection(sectionColumn, "C", "Character (Caractère)",
                    "Comment décririez-vous la douleur ?", socrate.Character);

                AddSocrateSection(sectionColumn, "R", "Radiation",
                    "La douleur se propage-t-elle ailleurs ?", socrate.Radiation);

                AddSocrateSection(sectionColumn, "A", "Association",
                    "Autres symptômes associés ?", socrate.Association);

                AddSocrateSection(sectionColumn, "T", "Timing (Temporalité)",
                    "La douleur est-elle constante ou intermittente ?", socrate.Timing);

                AddSocrateSection(sectionColumn, "E", "Exacerbating Factor",
                    "Qu'est-ce qui aggrave la douleur ?", socrate.ExacerbatingFactor);

                AddSocrateSection(sectionColumn, "R", "Relieving Factor",
                    "Qu'est-ce qui soulage la douleur ?", socrate.RelievingFactor);
            });
        }

        private void AddResultsPage(ColumnDescriptor column, List<PatientAnswerTests> tests,
            List<double> tintivValues, List<double> clinicalValues, Assessment assessment)
        {
            column.Item().Text("RÉSULTATS DU BILAN")
                .FontSize(16)
                .Bold()
                .FontColor(Colors.Blue.Darken2);

            column.Item().PaddingTop(10).LineHorizontal(2).LineColor(Colors.Blue.Lighten2);

            // Red Flags section
            column.Item().PaddingTop(15).Column(rfColumn =>
            {
                rfColumn.Item().Text("Analyse du risque (Red Flags)")
                    .FontSize(13)
                    .Bold()
                    .FontColor(Colors.Orange.Darken2);

                rfColumn.Item().PaddingTop(5)
                    .Background(Colors.Orange.Lighten4)
                    .Padding(10)
                    .Text($"Pourcentage de risque : {assessment.RedFlagsPercentage:F2} %")
                    .FontSize(14)
                    .Bold();
            });

            // Test done section
            column.Item().PaddingTop(15).Column(testsColumn =>
            {
                testsColumn.Item().Text("Tests effectués")
                    .FontSize(13)
                    .Bold()
                    .FontColor(Colors.Blue.Darken1);

                if (tests != null && tests.Any())
                {
                    var testsByCluster = tests.GroupBy(t =>
                        t.IsCustomTest ? "Tests personnalisés" :
                        t.Question?.Cluster?.Name ?? "Autres");

                    foreach (var clusterGroup in testsByCluster)
                    {
                        testsColumn.Item().PaddingTop(10).Column(clusterCol =>
                        {
                            clusterCol.Item().Text($"▸ {clusterGroup.Key} ({clusterGroup.Count()})")
                                .FontSize(12)
                                .Bold()
                                .FontColor(Colors.Blue.Darken2);

                            foreach (var test in clusterGroup)
                            {
                                clusterCol.Item().PaddingLeft(15).PaddingTop(5).Column(testCol =>
                                {
                                    testCol.Item().Text(test.IsCustomTest ? test.CustomTestName : test.Question?.Title ?? "Test")
                                        .FontSize(10)
                                        .Bold();

                                    var resultText = test.ResponseValue == "true" ? "✓ Positif" :
                                                   test.ResponseValue == "false" ? "✗ Négatif" :
                                                   test.ResponseValue;

                                    testCol.Item().Text($"Résultat : {resultText}")
                                        .FontSize(10)
                                        .FontColor(Colors.Grey.Darken1);

                                    if (!string.IsNullOrWhiteSpace(test.Observations))
                                    {
                                        testCol.Item().Text($"💬 {test.Observations}")
                                            .FontSize(9)
                                            .Italic()
                                            .FontColor(Colors.Grey.Medium);
                                    }
                                });
                            }
                        });
                    }
                }
                else
                {
                    testsColumn.Item().PaddingTop(5).Text("Aucun test enregistré")
                        .FontSize(10)
                        .Italic()
                        .FontColor(Colors.Grey.Medium);
                }
            });

            // Clinical data section
            column.Item().PaddingTop(15).Column(dataColumn =>
            {
                dataColumn.Item().Text("Données cliniques")
                    .FontSize(13)
                    .Bold()
                    .FontColor(Colors.Blue.Darken1);

                // Tintiv
                if (tintivValues != null && tintivValues.Any())
                {
                    dataColumn.Item().PaddingTop(10).Column(tintivCol =>
                    {
                        tintivCol.Item().Text("TINTIV (6 catégories)")
                            .FontSize(11)
                            .Bold();

                        var tintivCategories = new[] { "T", "I", "N", "T", "I", "V" };
                        for (int i = 0; i < Math.Min(tintivValues.Count, 6); i++)
                        {
                            tintivCol.Item().Text($"{tintivCategories[i]} : {tintivValues[i]:F2}")
                                .FontSize(10);
                        }
                    });
                }

                // Clinical profile (9 category)
                if (clinicalValues != null && clinicalValues.Any())
                {
                    dataColumn.Item().PaddingTop(10).Column(clinicalCol =>
                    {
                        clinicalCol.Item().Text("Profil clinique (9 catégories)")
                            .FontSize(11)
                            .Bold();

                        var clinicalCategories = new[] { "Cat 1", "Cat 2", "Cat 3", "Cat 4", "Cat 5", "Cat 6", "Cat 7", "Cat 8", "Cat 9" };
                        for (int i = 0; i < Math.Min(clinicalValues.Count, 9); i++)
                        {
                            clinicalCol.Item().Text($"{clinicalCategories[i]} : {clinicalValues[i]:F2}")
                                .FontSize(10);
                        }
                    });
                }
            });

            // Suspected pathology (Hardcoded value for now)
            column.Item().PaddingTop(15).Column(pathoColumn =>
            {
                pathoColumn.Item().Text("Pathologies suspectées")
                    .FontSize(13)
                    .Bold()
                    .FontColor(Colors.Blue.Darken1);

                pathoColumn.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(1);
                    });

                    AddPathologyRow(table, "Tumoral", "26%");
                    AddPathologyRow(table, "Infectieux", "10%");
                    AddPathologyRow(table, "Neurologique", "12%");
                    AddPathologyRow(table, "Traumatique", "8%");
                    AddPathologyRow(table, "Inflammatoire", "7%");
                    AddPathologyRow(table, "Vasculaire", "32%");
                });
            });
        }

        private void AddTableRow(TableDescriptor table, string label, string? value)
        {
            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                .Text(label).FontSize(10).Bold().FontColor(Colors.Grey.Darken1);

            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                .Text(value ?? "Non renseigné").FontSize(10);
        }

        private void AddMedicalInfo(ColumnDescriptor column, string label, string? value)
        {
            column.Item().PaddingTop(8).Column(col =>
            {
                col.Item().Text(label)
                    .FontSize(11)
                    .Bold()
                    .FontColor(Colors.Blue.Darken1);

                col.Item().PaddingTop(3)
                    .BorderLeft(2)
                    .BorderColor(Colors.Blue.Lighten3)
                    .PaddingLeft(10)
                    .Text(string.IsNullOrWhiteSpace(value) ? "Non renseigné" : value)
                    .FontSize(10)
                    .FontColor(string.IsNullOrWhiteSpace(value) ? Colors.Grey.Medium : Colors.Black);
            });
        }

        private void AddSocrateSection(ColumnDescriptor column, string badge, string title, string description, string? content)
        {
            column.Item().Column(sectionColumn =>
            {
                sectionColumn.Item().Row(row =>
                {
                    row.AutoItem()
                        .Background(Colors.Blue.Lighten3)
                        .Padding(5, 3)
                        .Text(badge)
                        .FontSize(10)
                        .Bold()
                        .FontColor(Colors.White);

                    row.RelativeItem()
                        .PaddingLeft(10)
                        .Text(title)
                        .FontSize(11)
                        .Bold()
                        .FontColor(Colors.Blue.Darken2);
                });

                sectionColumn.Item().PaddingTop(3)
                    .Text(description)
                    .FontSize(9)
                    .Italic()
                    .FontColor(Colors.Grey.Darken1);

                sectionColumn.Item().PaddingTop(5)
                    .BorderLeft(2)
                    .BorderColor(Colors.Blue.Lighten3)
                    .PaddingLeft(10)
                    .Text(string.IsNullOrWhiteSpace(content) ? "Non renseigné" : content)
                    .FontSize(10)
                    .FontColor(string.IsNullOrWhiteSpace(content) ? Colors.Grey.Medium : Colors.Black);
            });
        }

        private void AddPathologyRow(TableDescriptor table, string pathology, string percentage)
        {
            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8)
                .Text(pathology).FontSize(10).Bold();

            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8)
                .AlignCenter()
                .Text(percentage).FontSize(10).Bold().FontColor(Colors.Blue.Darken2);
        }
    }
}
