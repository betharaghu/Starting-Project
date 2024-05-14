namespace Starting_Project.Models
{
    public class ApplicationFormDto
    {
        public string ProgramName { get; set; } = string.Empty;
        public List<QuestionDto>? Questions { get; set; }
    }

    public class QuestionDto
    {
        public Guid Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public QuestionType Type { get; set; }
        public List<string>? Options { get; set; }
    }
}
