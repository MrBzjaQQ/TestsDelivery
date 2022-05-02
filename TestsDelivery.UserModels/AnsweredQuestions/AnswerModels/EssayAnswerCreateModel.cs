namespace TestsDelivery.UserModels.AnsweredQuestions.AnswerModels
{
    public class EssayAnswerCreateModel
    {
        public string Text { get; set; }

        public long ScheduledTestId { get; set; }

        public long CandidateId { get; set; }

        public long QuestionId { get; set; }
    }
}
