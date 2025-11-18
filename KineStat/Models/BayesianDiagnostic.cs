using System;
using System.Collections.Generic;

namespace KineStat.Models
{
    public class BayesianDiagnostic
    {
        // The methods will be included in a next sprint, it's just the base class with attributes, they might change

        public int Id { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int PathologyId { get; set; }
        public Pathology Pathology { get; set; }

        public double PriorProbability { get; set; }    // Le prior

        public double PosteriorProbability { get; set; }    // Le posterior

        public List<double> LikelihoodRatios { get; set; }  // RV+ & RV-

        public List<double> ProbabilityHistory { get; set; }    // Historique de probabilité après chaque tests, permet de suivre l'évolution

        public List<int> QuestionIds { get; set; }  // ID des questions/tests effectués

        public DateTime DiagnosticDate { get; set; }

        public int BilanId { get; set; }

        public BayesianDiagnostic()
        {
            LikelihoodRatios = new List<double>();
            ProbabilityHistory = new List<double>();
            QuestionIds = new List<int>();
            DiagnosticDate = DateTime.Now;
        }
    }
}