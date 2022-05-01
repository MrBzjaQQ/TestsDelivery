namespace TestsDelivery.UserModels.AnsweredQuestions.AnswerModels
{
    public class EssayAnswerModel
    {
        public string Text { get; set; }

        public long ScheduledTestId { get; set; }

        public long CandidateId { get; set; }
    }
}
