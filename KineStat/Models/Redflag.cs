namespace KineStat.Models
{
    public class Redflag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public int SeverityLevel { get; set; } 

        public virtual ICollection<Pathology> Pathologies { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}