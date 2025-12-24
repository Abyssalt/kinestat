using KineStat.Models;
using KineStat.Models.DTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

// Note : This is the base of the PDF Export, we can't do really better at the moment, so, the graphs and other diagrams have to be added to refactor this functionnality.

namespace KineStat.Services
{
    public class AssessmentCompletePdfService
    {
        /// <summary>
        /// Generate a complete PDF medical report for a patient assessment
        /// </summary>
        /// <param name="patient">The patient information</param>
        /// <param name="socrate">The SOCRATE data</param>
        /// <param name="assessment">The assessment data</param>
        /// <param name="tests">The list of tests performed</param>
        /// <param name="tintivValues">TINTIV Values</param>
        /// <param name="clinicalValues">Clinical profile values</param>
        /// <returns>An array containing the pdf document, with a size of 1 page</returns>
        public byte[] GenerateCompletePdf(
            Patient patient,
            Socrate? socrate,
            Assessment assessment,
            List<PatientAnswerTests> tests,
            List<double> tintivValues,
            List<double> clinicalValues,
            List<DetectedPathologyDTO> detectedPathologies)
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
                        .Background(Colors.Blue.Lighten3)
                        .Padding(8)
                        .Row(row =>
                        {
                            row.RelativeItem().Text("RAPPORT MÉDICAL - BILAN KINÉSITHÉRAPIE")
                                .FontSize(12)
                                .Bold()
                                .FontColor(Colors.Blue.Darken2);

                            row.AutoItem().Text($"{patient.LastName} {patient.FirstName} | {assessment.Date:dd/MM/yyyy}")
                                .FontSize(9)
                                .FontColor(Colors.Grey.Darken2);
                        });

                    // Start of the content
                    page.Content()
                        .PaddingVertical(5)
                        .Column(column =>
                        {
                            column.Spacing(8);

                            // Compact synthesis
                            AddCompactSynthesis(column, patient, assessment);

                            // Red Flags
                            AddRedFlagsSection(column, assessment);

                            // TINTIV Data
                            AddTintivData(column, tintivValues);

                            // Clinical profile data
                            AddClinicalData(column, clinicalValues);

                            // Add suspected pathology
                            AddPathologiesSection(column, detectedPathologies);
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
                        });
                });
            });

            return document.GeneratePdf();
        }

        /// <summary>
        /// Add the patient identification with administrative information for the doctor
        /// </summary>
        /// <param name="column">Where content will be added</param>
        /// <param name="patient">Patient information, including name, birth date, contact details and medical history</param>
        /// <param name="assessment">The assessment with the evaluation date</param>
        private void AddCompactSynthesis(ColumnDescriptor column, Patient patient, Assessment assessment)
        {
            column.Item().Text("IDENTIFICATION DU PATIENT")
                .FontSize(12)
                .Bold()
                .FontColor(Colors.Blue.Darken2);

            column.Item().PaddingTop(5).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                });

                // Line 1
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text("Nom").FontSize(9).Bold().FontColor(Colors.Grey.Darken1);
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text(patient.LastName).FontSize(9);
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text("Prénom").FontSize(9).Bold().FontColor(Colors.Grey.Darken1);
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text(patient.FirstName).FontSize(9);

                // Line 2
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text("Date de naissance").FontSize(9).Bold().FontColor(Colors.Grey.Darken1);
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text(patient.BirthDate.ToString("dd/MM/yyyy")).FontSize(9);
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text("N° Sécurité sociale").FontSize(9).Bold().FontColor(Colors.Grey.Darken1);
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text(patient.SocialSecurityNumber ?? "Non renseigné").FontSize(9);

                // Line 3
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text("Téléphone").FontSize(9).Bold().FontColor(Colors.Grey.Darken1);
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text(patient.PhoneNumber ?? "Non renseigné").FontSize(9);
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text("Email").FontSize(9).Bold().FontColor(Colors.Grey.Darken1);
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text(patient.Email ?? "Non renseigné").FontSize(9);

                // Line 4
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text("Date du bilan").FontSize(9).Bold().FontColor(Colors.Grey.Darken1);
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text(assessment.Date.ToString("dd/MM/yyyy")).FontSize(9);
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text("Statut").FontSize(9).Bold().FontColor(Colors.Grey.Darken1);
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                    .Text(patient.Status.ToString()).FontSize(9);
            });

            if (!string.IsNullOrWhiteSpace(patient.MedicalHistory))
            {
                column.Item().PaddingTop(5).Column(col =>
                {
                    col.Item().Text("Antécédents médicaux")
                        .FontSize(10)
                        .Bold()
                        .FontColor(Colors.Blue.Darken1);
                    col.Item().PaddingTop(2)
                        .BorderLeft(2)
                        .BorderColor(Colors.Blue.Lighten3)
                        .PaddingLeft(8)
                        .Text(patient.MedicalHistory)
                        .FontSize(9);
                });
            }
        }

        /// <summary>
        /// Add the RedFlag section with a progress bar (like the gauge)
        /// </summary>
        /// <param name="column">Where content will be added</param>
        /// <param name="assessment">The assessment with the evaluation date</param>
        private void AddRedFlagsSection(ColumnDescriptor column, Assessment assessment)
        {
            column.Item().Text("ANALYSE DU RISQUE")
                .FontSize(12)
                .Bold()
                .FontColor(Colors.Orange.Darken2);

            var percentage = assessment.RedFlagsPercentage ?? 0;
            var barColor = percentage < 30 ? Colors.Green.Medium :
                          percentage < 60 ? Colors.Orange.Medium :
                          Colors.Red.Medium;

            column.Item().PaddingTop(5).Row(row =>
            {
                row.RelativeItem().Height(20).Background(Colors.Grey.Lighten2).Row(innerRow =>
                {
                    innerRow.ConstantItem((float)percentage * 4.5f).Background(barColor);
                    innerRow.RelativeItem();
                });

                row.AutoItem().PaddingLeft(10).AlignMiddle()
                    .Text($"{percentage:F1}%")
                    .FontSize(11)
                    .Bold()
                    .FontColor(barColor);
            });

            column.Item().PaddingTop(3).Text("Niveau de vigilance recommandé")
                .FontSize(9)
                .Italic()
                .FontColor(Colors.Grey.Darken1);
        }

        /// <summary>
        /// Add the suspected pathologies section
        /// </summary>
        /// <param name="column">Where content will be added</param>
        private void AddPathologiesSection(ColumnDescriptor column, List<DetectedPathologyDTO> detectedPathologies)
        {
            column.Item().Text("PATHOLOGIES SUSPECTÉES")
                .FontSize(12)
                .Bold()
                .FontColor(Colors.Blue.Darken1);

            if (detectedPathologies != null && detectedPathologies.Any())
            {
                column.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(1);
                    });

                    foreach (var pathology in detectedPathologies.OrderByDescending(p => p.PathologyProbability))
                    {
                        AddPathologyRow(table, pathology.PathologyName, $"{pathology.PathologyProbability * 100:F1}%");
                    }
                });
            }
            else
            {
                column.Item().PaddingTop(5).Text("Aucune pathologie détectée")
                    .FontSize(9)
                    .Italic()
                    .FontColor(Colors.Grey.Medium);
            }
        }

        /// <summary>
        /// Add the TINTIV data with values and progress bars for RedFlags categories
        /// </summary>
        /// <param name="column">Where content will be added</param>
        /// <param name="tintivValues">The list of TINTIV values</param>
        private void AddTintivData(ColumnDescriptor column, List<double> tintivValues)
        {
            column.Item().Text("TINTIV - Données Red Flags")
                .FontSize(12)
                .Bold()
                .FontColor(Colors.Red.Darken1);

            if (tintivValues != null && tintivValues.Any())
            {
                column.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    var labels = new[] { "Tumeur", "Infection", "Neurologique", "Traumatisme", "Inflammatoire", "Vasculaire" };

                    for (int i = 0; i < Math.Min(tintivValues.Count, 6); i++)
                    {
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                            .Text(labels[i]).FontSize(9).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                            .AlignCenter().Text($"{tintivValues[i]:F1} / 5").FontSize(9);

                        // Progress bar
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                            .Height(15).Background(Colors.Grey.Lighten3).Row(barRow =>
                            {
                                barRow.ConstantItem((float)tintivValues[i] * 30f).Background(Colors.Red.Lighten2);
                                barRow.RelativeItem();
                            });
                    }
                });
            }
        }

        /// <summary>
        /// Adds the clinical profile data with values and progress bars for the 9 given categories
        /// </summary>
        /// <param name="column">Where content will be added</param>
        /// <param name="clinicalValues">The list of clinical profile values, for maximum 9 given categories</param>
        private void AddClinicalData(ColumnDescriptor column, List<double> clinicalValues)
        {
            column.Item().Text("PROFIL CLINIQUE - 9 Catégories")
                .FontSize(12)
                .Bold()
                .FontColor(Colors.Blue.Darken1);

            if (clinicalValues != null && clinicalValues.Any())
            {
                column.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(2);
                    });

                    var labels = new[] {
                        "Articulaire/structurel", "Myofascial", "Nociceptif",
                        "Neuropathique", "Nociplastique", "Contrôle sensorimoteur",
                        "Croyance & cognition", "Socio-environnemental", "Émotionnel/Affectif"
                    };

                    for (int i = 0; i < Math.Min(clinicalValues.Count, 9); i++)
                    {
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                            .Text(labels[i]).FontSize(9).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                            .AlignCenter().Text($"{clinicalValues[i]:F1} / 5").FontSize(9);

                        // Progress bar
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                            .Height(15).Background(Colors.Grey.Lighten3).Row(barRow =>
                            {
                                barRow.ConstantItem((float)clinicalValues[i] * 30f).Background(Colors.Blue.Lighten2);
                                barRow.RelativeItem();
                            });
                    }
                });
            }
        }

        /*  Commented because it could be useful in a next refactor
        /// <summary>
        /// Add a row to a table with a label and corresponding value
        /// </summary>
        /// <param name="table">The table descriptor</param>
        /// <param name="label">The label text</param>
        /// <param name="value">The value text</param>
        private void AddTableRow(TableDescriptor table, string label, string? value)
        {
            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                .Text(label).FontSize(10).Bold().FontColor(Colors.Grey.Darken1);

            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                .Text(value ?? "Non renseigné").FontSize(10);
        }
        */

        /*  Commented because it could be useful in a next refactor
        /// <summary>
        /// Add medical information with a label
        /// </summary>
        /// <param name="column">Where content will be added</param>
        /// <param name="label">The label text</param>
        /// <param name="value">The medical information test</param>
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
        */

        /// <summary>
        /// Add a pathology row with the name and the percentage to the table
        /// </summary>
        /// <param name="table">The table descriptor</param>
        /// <param name="pathology"></param>
        /// <param name="percentage"></param>
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
