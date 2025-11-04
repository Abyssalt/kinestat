using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{
    public abstract class Question
    {

            public int Id { get; set; }


            public string Title { get; set; }

            public string Type { get; set; }


            public double RVPositif { get; set; } = 0;


            public double RVNegatif { get; set; } = 0;

            public Answer answer { get; set; }

            public int? ClusterId { get; set; }

            public virtual Cluster Cluster { get; set; }

        public abstract bool Validate();


            public bool isAnswered()
            {
                return answer != null;
            }
        }

    }

