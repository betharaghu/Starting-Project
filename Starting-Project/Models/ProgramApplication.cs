namespace Starting_Project.Models
{
    public class ProgramApplication
    {
        public Guid Id { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public List<Question>? Questions { get; set; }
    }

    public class Question
    {
        public Guid Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public QuestionType Type { get; set; }
        public List<string>? Options { get; set; }  // Used for dropdown/multiple choice
    }

    public enum QuestionType
    {
        Paragraph,
        YesNo,
        Dropdown,
        MultipleChoice,
        Date,
        Number
    }
}
