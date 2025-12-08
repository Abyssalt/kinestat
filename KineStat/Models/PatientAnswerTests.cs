using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{
    public class PatientAnswerTests
    {


        [Key]
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        public int? QuestionId { get; set; }  
        [ForeignKey("QuestionId")]
        public virtual Question? Question { get; set; }

        public int? AnswerId { get; set; }  
        [ForeignKey("AnswerId")]
        public virtual Answer? Answer { get; set; }

        public int AssessmentId { get; set; }
        [ForeignKey("AssessmentId")]
        public Assessment Assessment { get; set; }

        [Required]
        public DateTime DateResponse { get; set; }

       
        [MaxLength(1000)]
        public string? ResponseValue { get; set; }  

        [MaxLength(2000)]
        public string? Observations { get; set; }

        public bool IsCustomTest { get; set; } = false;

        [MaxLength(200)]
        public string? CustomTestName { get; set; }

        [MaxLength(20)]
        public string? CustomTestType { get; set; }  
    }
}
