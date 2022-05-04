using TestsDelivery.UserModels.Subject;

namespace TestsDelivery.UserModels.Questions
{
    public record QuestionInSubjectModel
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public QuestionType Type { get; set; }
    }
}
