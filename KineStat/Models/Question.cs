using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{
    public abstract class Question
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public double RVPositive { get; set; } = 0;

        public double RVNegative { get; set; } = 0;

        [Required]
        public string SourceRv { get; set; }

        public int CategoryId { get; set; }

        public int? ClusterId { get; set; }

        public virtual Cluster? Cluster { get; set; }

        public virtual Category? Category { get; set; }

        public virtual ICollection<QuestionPathology> QuestionPathologies { get; set; } = new List<QuestionPathology>();
        public virtual ICollection<PatientAnswerTests>? PatientResponses { get; set; }

    }
}

