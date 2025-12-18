using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    [Index(nameof(QuestionId), nameof(PathologyId), IsUnique = true)]
    public class QuestionPathology
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public int PathologyId { get; set; }   
        
        public Pathology Pathology { get; set; }
        public Question Question {  get; set; }
    }
}
