namespace KineStat.Models
{
    public class BayesCalculator
    {
        /// <summary>
        /// Calculates the posterior probability using Bayes theorem
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if prior is not between 0 and 1 or RV+ / RV- ≤ 0.</exception>
        public double CalculatePosterior(double prior, double rvPlus, double rvMinus, bool answer)
        {
            if (prior <= 0) prior = 0.001;
            rvPlus = rvPlus <= 0 ? 0.001 : rvPlus;
            rvMinus = rvMinus <= 0 ? 0.001 : rvMinus;

            double oddPrior = prior / (1 - prior);
            double oddPost = 0;
            if (answer)
            {
                oddPost = oddPrior * rvPlus;

            }else
            {
                oddPost = oddPrior * rvMinus;
            }
            //Convert odd to actual probability
            double posterior = oddPost/ (1 + oddPost);
            
            return posterior;

        }

        /// <summary>
        /// Calculates the posterior probability for a category by applying Bayes' theorem sequentially to each answer.</summary>
        /// <param name="answers" > The list of boolean patient answers belonging to this category </param>
        /// <param name="categoryPrior"> The initial prior probability of the category (must be between 0 and 1) </param>
        /// <returns>
        /// The final posterior probability after processing all answers
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if any RV+ or RV– value is ≤ 0, or if the prior is not between 0 and 1
        /// </exception>
        public double CalculateCategoryProbability(List<PatientAnswerBool> answers, double categoryPrior)
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
