namespace QuestionManagementService.Domain.Entities;

public class Question
{
    public Guid Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public byte Difficulty { get; set; }

    public Guid QuestionBankId { get; set; }

    public QuestionBank? QuestionBank { get; set; }

    public ICollection<AnswerOption> AnswerOptions { get; set; } = [];

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }
}
