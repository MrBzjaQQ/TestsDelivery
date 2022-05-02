using TestsPortal.Domain.Questions;

namespace TestsPortal.Domain.AnsweredQuestions.QuestionWithAnswers
{
    public record MultipleChoiceAnswered : QuestionBase
    {
        public IEnumerable<AnswerOption> SelectedAnswers { get; set; }
    }
}
