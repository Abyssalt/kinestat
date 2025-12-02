using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KineStat.Models.DTO
{

    public class SaveExamenCliniqueDTO
    {
        public int PatientId { get; set; }
        public List<ExamenCliniqueResponseDTO> Responses { get; set; } = new List<ExamenCliniqueResponseDTO>();
    }

    public class ExamenCliniqueResponseDTO
    {
        public int QuestionId { get; set; }
        public string Response { get; set; } = string.Empty;
        public string? Notes { get; set; }
}

}
