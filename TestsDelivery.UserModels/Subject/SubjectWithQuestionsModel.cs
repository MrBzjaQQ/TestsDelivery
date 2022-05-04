using TestsDelivery.UserModels.Questions;

namespace TestsDelivery.UserModels.Subject
{
    public class SubjectWithQuestionsModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<QuestionInSubjectModel> Questions { get; set; }
    }
}
