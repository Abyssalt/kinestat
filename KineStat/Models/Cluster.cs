using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public class Cluster
    {
        [Key] public int Id { get; set; }

        [Required] public string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public List<QuestionModel> questions { get; set; } = new List<QuestionModel>();

        /// <summary>
        /// Vérifie si le cluster est complet (tous les tests obligatoires effectués)
        /// </summary>
        public bool EstComplet()
        {
            foreach (var test in questions)
            {
                if (test.answer == null)
                    return false;
            }

            return true;
        }
    
        /// <summary>
        /// Obtient la liste des résultats
        /// </summary>
        public List<Answer> GetResultats()
        {
            var resultats = new List<Answer>();
            foreach (var test in questions)
            {
                if (test.answer != null)
                {
                    resultats.Add(test.answer);
                }
            }
            return resultats;
        }

    }}
