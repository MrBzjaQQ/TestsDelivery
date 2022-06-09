using TestsPortal.Domain.Questions;

namespace TestsPortal.Domain.AnsweredQuestions.QuestionWithAnswers
{
    public record SingleChoiceAnswered : SingleChoiceQuestion
    {
        public AnswerOption SelectedAnswer { get; set; }
    }
}
