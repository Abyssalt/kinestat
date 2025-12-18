using KineStat.Data;
using KineStat.Models;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace KineStat.Services
{
    /// <summary>
    /// Service responsible for calculating posterior probabilities for pathologies and categories using Bayes' theorem.
    /// This service retrieves patient answers and clusters from the database, then orchestrates the probability calculations 
    /// by delegating the actual computation to BayeCalcluator>
    /// </summary>
    public class BayesService
    {
        private readonly KineDbContext _context;
        private readonly BayesCalculator _calculator;

        public BayesService (KineDbContext context, BayesCalculator calculator)
        {
            _context = context;
            _calculator = calculator;
        }

        /// <summary>
        /// Calculates the posterior probability for a category by retrieving patient answers
        /// from the database and applying Bayes' theorem sequentially.
        /// </summary>
        /// <param name="patientId">ID of the patient.</param>
        /// <param name="assessmentId">ID of the assessment.</param>
        /// <param name="categoryId">ID of the question category.</param>
        /// <param name="categoryPrior">The initial prior probability of the category (must be between 0 and 1).</param>
        /// <returns>The final posterior probability for the category.</returns>
        public async Task<double> CalculateCategoryProbability(int patientId, int assessmentId, int categoryId, double categoryPrior)
        {
            var answers = await _context.PatientAnswers
                .OfType<PatientAnswerBool>()
                .Where(a => a.PatientId == patientId && a.AssessmentId == assessmentId && a.Question.CategoryId == categoryId)
                .Include(a => a.Question)
                .ToListAsync();

            double posterior = categoryPrior;
            foreach (var answer in answers)
            {
                posterior = _calculator.CalculatePosterior(posterior, answer.Question.RVPositive, answer.Question.RVNegative, answer.Value);
            }
            return posterior;
        }



        /// <summary>
        /// Retrieves the list of patient answers that are not linked to any cluster for a specific pathology.
        /// </summary>
        /// <param name="patientId">ID of the patient.</param>
        /// <param name="assessmentId">ID of the assessment.</param>
        /// <param name="dossierId">ID of the dossier.</param>
        /// <param name="pathologyId">ID of the pathology.</param>
        /// <returns>A list of patient boolean answers for the specified pathology and no cluster.</returns>
        private async Task<List<PatientAnswerBool>> GetPatientAnswersByPathology(int patientId, int assessmentId, int pathologyId, int folderId)
        {
            var patient = _context.Patients
               .Where(p => p.Id == patientId)
               .FirstOrDefault();
            if (patient == null) return null;
            var assessment = _context.Assessments
                    .Where(a => a.PatientId == patientId && a.Id == assessmentId && a.DossierId == folderId)
                    .FirstOrDefault();
            if (assessment == null)
            {
                return null;
            }
            var answerByPathology = await _context.PatientAnswers
                .OfType<PatientAnswerBool>()
                .Where(a => a.PatientId == patientId
                && a.AssessmentId == assessmentId
                && a.Question.QuestionPathologies.Any(p => p.PathologyId == pathologyId)
                && a.Question.ClusterId == null)
                .Include(a => a.Question)
                .ToListAsync();
            return answerByPathology;
        }

        /// <summary>
        /// Retrieves the list of clusters associated with a specific pathology for a patient and assessment.
        /// </summary>
        /// <param name="patientId">ID of the patient.</param>
        /// <param name="assessmentId">ID of the assessment.</param>
        /// <param name="folderId">ID of the folder </param>
        /// <param name="pathologyId">ID of the pathology.</param>
        /// <returns>A list of clusters associated with the specified pathology.</returns>
        private async Task<List<Cluster>> GetClustersByPathology(int patientId, int assessmentId, int pathologyId, int folderId)
        {
            var patient = await _context.Patients
              .Where(p => p.Id == patientId)
              .FirstOrDefaultAsync();
            if (patient == null) return null;
            var assessment = await _context.Assessments
                    .Where(a => a.PatientId == patientId && a.Id == assessmentId && a.DossierId == folderId)
                    .FirstOrDefaultAsync();
            if (assessment == null)
            {
                return null;
            }
            var clustersByPathology = await _context.Cluster
                .Where(c => c.MinNumberOfPositiveTests!= null && c.RVNegative!= null && c.RVPositive != null && c.Pathologies.Any(p => p.Id == pathologyId))
                .Include(p => p.Pathologies)

                .ToListAsync();
            return clustersByPathology;

        }


        /// <summary>
        /// Calculates the posterior probability for a single cluster based on the number of positive tests and the cluster's likelihood ratios.
        /// </summary>
        /// <param name="cluster">The cluster object containing its RV+ / RV- values and minimum positive tests.</param>
        /// <param name="prior">The prior probability before considering this cluster.</param>
        /// <returns>The posterior probability after evaluating the cluster.</returns>
        public async Task<double> CalculateClusterProbability(Cluster cluster, double prior, int patientId, int assessmentId)
        {

            int nbPositiveBools = await _context.PatientAnswerBools
                .Where(a => a.PatientId == patientId
                && a.AssessmentId == assessmentId
                &&a.Question.ClusterId == cluster.Id 
                && a.Value)
                .CountAsync();
            int nbPositiveTests = await _context.PatientAnswerTests
                .Where(a => a.PatientId == patientId
                &&a.AssessmentId == assessmentId
                && a.Question.ClusterId == cluster.Id
                && !string.IsNullOrEmpty(a.ResponseValue)
                && (a.ResponseValue.ToLower() == "true" || a.ResponseValue.ToLower() == "oui"))
                .CountAsync();


            bool clusterResult = nbPositiveBools + nbPositiveTests >= cluster.MinNumberOfPositiveTests;

            return _calculator.CalculatePosterior(prior, cluster.RVPositive?? 1, cluster.RVNegative?? 1, clusterResult);

        }


        /// <summary>
        /// Calculates the posterior probability for a pathology by combining non-cluster answers and all associated clusters.
        /// </summary>
        /// <param name="prior">The initial prior probability of the pathology.</param>
        /// <param name="patientId">ID of the patient.</param>
        /// <param name="assessmentId">ID of the assessment.</param>
        /// <param name="dossierId">ID of the dossier.</param>
        /// <param name="pathologyId">ID of the pathology.</param>
        /// <returns>The final posterior probability for the pathology after evaluating all answers and clusters.</returns>
        public async Task<double> CalculateProbabilityByPathology(double prior, int patientId, int assessmentId,int pathologyId, int folderId)
        {
            double posterior = prior;
            var boolAnswers = await GetPatientAnswersByPathology(patientId, assessmentId, pathologyId, folderId);
            if (boolAnswers != null && boolAnswers.Any())
            {
                foreach( var a in boolAnswers)
                {
                    posterior = _calculator.CalculatePosterior(posterior, a.Question.RVPositive, a.Question.RVNegative, a.Value);
                }
            
            }

            var clusters = await GetClustersByPathology(patientId, assessmentId, pathologyId, folderId);
            if (clusters != null && clusters.Any())
            {
                foreach (var cluster in clusters)
                {
                    posterior = await CalculateClusterProbability(cluster, posterior, patientId, assessmentId);
                }
            }
            return posterior;

        }


   
    }
}