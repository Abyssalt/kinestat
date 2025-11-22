using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{
    public class MedicalRecord //This class contains every assessment of a patient
    {
        [Key]
        public int Id { get; set; }
        public int PatientId {  get; set; }
        public virtual ICollection<Assessment> Assessments { get; set; }
        public bool IsArchived { get; set; }
        [Required, DataType(DataType.Date), Column(TypeName = "ArchivedDate")]
        public DateTime ArchivedAt { get; set; }
    }
}
