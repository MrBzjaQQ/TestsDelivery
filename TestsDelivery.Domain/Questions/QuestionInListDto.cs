using TestsDelivery.Domain.Subject;

namespace TestsDelivery.Domain.Questions
{
    public class QuestionInListDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public QuestionType Type { get; set; }

        public SubjectInListDto Subject { get; set; }
    }
}
