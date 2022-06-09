using TestsPortal.Domain.Questions;

namespace TestsPortal.Domain.AnsweredQuestions.QuestionWithAnswers
{
    public record EssayAnswered : EssayQuestion
    {
        public string Text { get; set; }
    }
}
