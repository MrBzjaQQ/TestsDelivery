using System.Collections.Generic;
using TestsDelivery.BL.Models.Questions.AnswerOptions;
using TestsDelivery.BL.Models.Subject;

namespace TestsDelivery.BL.Models.Questions.MultipleChoice
{
    public record McqReadModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public SubjectReadModel Subject { get; set; }

        public IEnumerable<AnswerOptionReadModel> AnswerOptions { get; set; }
    }
}
