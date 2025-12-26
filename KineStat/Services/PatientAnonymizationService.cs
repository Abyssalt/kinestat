using KineStat.Data;
using KineStat.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KineStat.Services
{
    /// <summary>
    /// Service responsible for the automatic anonymization of patient data 
    /// in accordance with the GDPR (20 years of inactivity)
    /// </summary>
    public class PatientAnonymizationService
    {
        private readonly KineDbContext _context;
        private readonly ILogger<PatientAnonymizationService> _logger;
        private const int RETENTION_YEARS = 20;

        public PatientAnonymizationService(
            KineDbContext context,
            ILogger<PatientAnonymizationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Anonymizes all patients whose status has been Inactive for more than 20 years.
        /// Returns the number of anonymized patients.
        /// </summary>
        public async Task<int> AnonymizeExpiredPatientsAsync()
        {
            _logger.LogInformation("=== DÉBUT ANONYMISATION RGPD ===");

            try
            {
                var cutoffDate = DateTime.UtcNow.AddYears(-RETENTION_YEARS);

                var patientsToAnonymize = await _context.Patients
                    .Where(p => p.Status == PatientStatus.Inactif
                                && p.InactiveSinceDate != null
                                && p.InactiveSinceDate.Value < cutoffDate
                                && !p.IsAnonymized)
                    .ToListAsync();

                _logger.LogInformation($"Patients inactifs depuis plus de {RETENTION_YEARS} ans à anonymiser: {patientsToAnonymize.Count}");

                if (patientsToAnonymize.Count == 0)
                {
                    _logger.LogInformation("Aucun patient à anonymiser");
                    return 0;
                }

                int anonymizedCount = 0;
                var now = DateTime.UtcNow;

                foreach (var patient in patientsToAnonymize)
                {
                    try
                    {
                        AnonymizePatient(patient, now);
                        anonymizedCount++;

                        _logger.LogInformation(
                            $"Patient ID {patient.Id} anonymisé (inactif depuis le {patient.InactiveSinceDate:yyyy-MM-dd})");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Erreur anonymisation patient ID {patient.Id}");
                    }
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    $"=== FIN ANONYMISATION RGPD : {anonymizedCount} patients anonymisés ===");

                return anonymizedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERREUR lors de l'anonymisation");
                throw;
            }
        }

        /// <summary>
        /// Anonymizes a patient’s personal data and changes their status to Done
        /// </summary>
        private void AnonymizePatient(Patient patient, DateTime anonymizationDate)
        {
            var anonymousId = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

            patient.LastName = $"ANONYME_{anonymousId}";
            patient.FirstName = "ANONYME";
            patient.Email = $"anonyme_{anonymousId}@anonymise.rgpd";
            patient.PhoneNumber = "0000000000";
            patient.SocialSecurityNumber = $"ANONYMISE_{anonymousId}";
            patient.Address = "ADRESSE ANONYMISEE";

            patient.Profession = null;
            patient.PhysicalActivities = null;
            patient.MedicalHistory = "DONNEES ANONYMISEES";
            patient.ActualMedication = null;

            patient.Status = PatientStatus.Anonymisé;
            patient.IsAnonymized = true;
            patient.AnonymizedDate = anonymizationDate;

            // Note: BirthDate, Weight, Height, Gender sont conservés pour les statistiques
        }
    }
}