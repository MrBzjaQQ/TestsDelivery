namespace TestsDelivery.UserModels.Questions.BaseQuestion
{
    public class QuestionInListModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public SubjectInList Subject { get; set; }
    }
}
