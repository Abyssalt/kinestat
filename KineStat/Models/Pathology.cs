using KineStat.Models;

namespace KineStat.Models {

    public class Pathology
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Prior { get; set; }

        public int? CategorieId { get; set; }
        public virtual Category? Category { get; set; }

        public int? RedFlagId { get; set; }
        public virtual Redflag? RedFlag { get; set; }
    }

}