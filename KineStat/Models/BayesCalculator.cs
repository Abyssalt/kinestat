using KineStat.Data;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

namespace KineStat.Models
{
    public class BayesCalculator
    {
       
        /// <summary>
        /// Calculates the posterior probability using Bayes theorem
        /// </summary>
        public double CalculatePosterior(double prior, double rvPlus, double rvMinus, bool answer)
        {
            if (prior <= 0) prior = 0.001;
            rvPlus = rvPlus <= 0 ? 1 : rvPlus;
            rvMinus = rvMinus <= 0 ? 1 : rvMinus;

            double oddPrior = prior / (1 - prior);
            double oddPost = 0;
            if (answer)
            {
                oddPost = oddPrior * rvPlus;

            }
            else
            {
                oddPost = oddPrior * rvMinus;
            }
            //Convert odd to actual probability
            double posterior = oddPost / (1 + oddPost);

            return posterior;

        }

        //This method is used for tests only
        public double CalculateCategoryProbability (List<PatientAnswerBool> answers, double categoryPrior)
        {
            double posterior = categoryPrior;
            foreach (var answer in answers)
            {
                posterior = CalculatePosterior(posterior, answer.Question.RVPositive, answer.Question.RVNegative, answer.Value);
            }
            return posterior;

        }
      

    }
}
