namespace KineStat.Models
{
    public class Redflag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public RedFlagCategory Category { get; set; }

        public int SeverityLevel { get; set; } // 1-5

        // Navigation properties
        public virtual ICollection<Pathology> Pathologies { get; set; }
        public virtual ICollection<Bilan> Bilans { get; set; }
    }

    public enum RedFlagCategory
    {
        Tumeur,
        Infection,
        Neurologique,
        Traumatisme,
        Inflammatoire,
        Vasculaire
    }
}