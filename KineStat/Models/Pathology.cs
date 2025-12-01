using KineStat.Models;

namespace KineStat.Models {

    public class Pathology
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Prior { get; set; }

        public virtual List<Question> Questions { get; set; } 

        public int? RedFlagId { get; set; }

        public virtual Redflag? RedFlag { get; set; }
    }
}