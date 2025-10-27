using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{
    public class QuestionModel
    {

        public abstract class Question
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required(ErrorMessage = "L'intitulé est obligatoire")]
            [StringLength(500)]
            public string Intitule { get; set; }

            [Required(ErrorMessage = "Le type est obligatoire")]
            [StringLength(50)]
            public string Type { get; set; }

            [Display(Name = "RV+")]
            public int RVPositif { get; set; } = 0;

            [Display(Name = "RV-")]
            public int RVNegatif { get; set; } = 0;

            public virtual Answer answer { get; set; }

            public abstract bool Valider();


            public bool EstRepondue()
            {
                return Answer != null;
            }
        }

    }
}
