using KineStat.Models;

namespace KineStat.Models {

    public class Pathology
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Prior { get; set; }


        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    }

}