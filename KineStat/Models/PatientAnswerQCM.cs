namespace KineStat.Models
{
    public class PatientAnswerQCM : PatientAnswer
    {
        public int AnswerId { get; set; } 

        public virtual Answer ChosenAnswer { get; set; }
    }
}
