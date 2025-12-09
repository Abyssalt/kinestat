namespace KineStat.Models.DTO
{
    public class SaveTestResultsDTO
    {
        public int PatientId { get; set; }
        public int? AssessmentId { get; set; }

        public List<TestResponseDTO> Tests { get; set; } = new List<TestResponseDTO>();
    }

    public class TestResponseDTO
    {
        public int Id { get; set; } 
        public string Value { get; set; } = string.Empty;
        public string? Observations { get; set; }
        public bool Custom { get; set; } = false;

        // Pour tests personnalisés
        public string? Name { get; set; }
        public string? Type { get; set; }
    }
}