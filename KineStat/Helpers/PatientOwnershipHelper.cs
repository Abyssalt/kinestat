using KineStat.Data;
using Microsoft.EntityFrameworkCore;

namespace KineStat.Helpers
{
    /// <summary>
    /// Helper methods to verify physiotherapist ownership of patient data.
    /// </summary>
    public static class PatientOwnershipHelper
    {
        /// <summary>
        /// Checks if a patient belongs to a specific physiotherapist.
        /// </summary>
        public static async Task<bool> IsPatientOwnedByPhysio(KineDbContext context, int physioId, int patientId)
        {
            var patient = await context.Patients
                .FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
                return false;

            return patient.PhysioId == physioId;
        }

        /// <summary>
        /// Checks if an assessment's patient belongs to a specific physiotherapist.
        /// </summary>
        public static async Task<bool> IsAssessmentOwnedByPhysio(KineDbContext context, int physioId, int assessmentId)
        {
            var assessment = await context.Assessments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == assessmentId);

            if (assessment == null || assessment.Patient == null)
                return false;

            return assessment.Patient.PhysioId == physioId;
        }

        /// <summary>
        /// Checks if a "dossier patient" belongs to a specific physiotherapist.
        /// </summary>
        public static async Task<bool> IsDossierOwnedByPhysio(KineDbContext context, int physioId, int dossierId)
        {
            var dossier = await context.Dossiers
                .Include(d => d.Patient)
                .FirstOrDefaultAsync(d => d.Id == dossierId);

            if (dossier == null || dossier.Patient == null)
                return false;

            return dossier.Patient.PhysioId == physioId;
        }
    }
}