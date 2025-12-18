using KineStat.Models;

namespace KineStatUnitTests
{
    [TestClass]
    public sealed class BayesCalculatorTests
    {
        private BayesCalculator _calculator;

        [TestInitialize]
        public void Setup()
        {
            _calculator = new BayesCalculator();
        }

        [TestMethod]
        public void CalculatePosterior_NominalCase_AddedValues()
        {
            // Arrange
            double prior = 0.5;
            double rvPlus = 2.0;
            double rvMinus = 0.5;
            bool answer = true;

            double expected = 2.0 / 3.0;

            // Act
            double result = _calculator.CalculatePosterior(prior, rvPlus, rvMinus, answer);

            // Assert
            Assert.AreEqual(expected, result, 0.0001);
        }

        [TestMethod]
        public void CalculatePosterior_FalseAnswer_UsesRvMinus()
        {
            // Arrange
            double prior = 0.5;
            double rvPlus = 2.0;
            double rvMinus = 0.5;
            bool answer = false;

            double expected = 0.5 / 1.5;

            // Act
            double result = _calculator.CalculatePosterior(prior, rvPlus, rvMinus, answer);

            // Assert
            Assert.AreEqual(expected, result, 0.0001);
        }

        [TestMethod]
        public void CalculatePosterior_PriorZero_UsesMinimalValue()
        {
            // Arrange
            double prior = 0;
            double rvPlus = 2;
            double rvMinus = 1;
            bool answer = true;

            double oddPrior = 0.001 / 0.999;
            double expected = (oddPrior * 2) / (1 + oddPrior * 2);

            // Act
            double result = _calculator.CalculatePosterior(prior, rvPlus, rvMinus, answer);

            // Assert
            Assert.AreEqual(expected, result, 0.0001);
        }

        [TestMethod]
        public void CalculatePosterior_RvPlusZero_UsesDefaultValue()
        {
            // Arrange
            double prior = 0.5;
            double rvPlus = 0;
            double rvMinus = 1;
            bool answer = true;

            double expected = 0.5;

            // Act
            double result = _calculator.CalculatePosterior(prior, rvPlus, rvMinus, answer);

            // Assert
            Assert.AreEqual(expected, result, 0.0001);
        }


        [TestMethod]
        public void CalculateCategoryProbability_NominalCase_UpdatedPosterior()
        {
            // Arrange
            var answers = new List<PatientAnswerBool>
            {
                new PatientAnswerBool
                {
                    Value = true,
                    Question = new QuestionBool()
                    {
                        RVPositive = 2,
                        RVNegative = 0.5
                    }
                },
                new PatientAnswerBool
                {
                    Value = false,
                    Question = new QuestionBool()
                    {
                        RVPositive = 3,
                        RVNegative = 0.5
                    }
                }
            };

            double prior = 0.5;

            double expected = 0.5;

            // Act
            double result = _calculator.CalculateCategoryProbability(answers, prior);

            // Assert
            Assert.AreEqual(expected, result, 0.0001);
        }

        [TestMethod]
        public void CalculateCategoryProbability_NominalCase_TableValues_MatchesFinalPosterior()
        {
            // Arrange
            var answers = new List<PatientAnswerBool>
            {
                new PatientAnswerBool
                {
                    Value = true,
                    Question = new QuestionBool ()
                    {
                        RVPositive = 9.2,
                        RVNegative = 0.7
                    }
                },
                new PatientAnswerBool
                {
                    Value = false,
                    Question = new QuestionBool()
                    {
                        RVPositive = 5.5,
                        RVNegative = 1
                    }
                },
                new PatientAnswerBool
                {
                    Value = true, 
                    Question = new QuestionBool()
                    {
                        RVPositive = 2.6,
                        RVNegative = 0.5
                    }
                },
                new PatientAnswerBool
                {
                    Value = false, 
                    Question = new QuestionBool ()
                    {
                        RVPositive = 4,
                        RVNegative = 0.1
                    }
                }
            };

            double prior = 0.01010101;
            double expectedPosterior = 0.0236; 

            // Act
            double result = _calculator.CalculateCategoryProbability(answers, prior);

            // Assert
            Assert.AreEqual(expectedPosterior, result, 0.0005);
        }

    }
}
