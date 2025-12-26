using KineStat.Models;

namespace KineStat.Models {
    
    /// <summary>
    /// Represents a medical pathology, including its identifier, name, and associated questions.
    /// </summary>
    public class Pathology
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<QuestionPathology> QuestionPathologies { get; set; } = new List<QuestionPathology>();
    }
}