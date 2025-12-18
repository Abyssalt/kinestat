using KineStat.Models;

namespace KineStat.Models {

    public class Pathology
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<QuestionPathology> QuestionPathologies { get; set; } = new List<QuestionPathology>();
    }
}