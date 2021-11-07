using System.Collections.Generic;
using TestsDelivery.BL.Models.Questions.AnswerOptions;

namespace TestsDelivery.BL.Models.Questions.SingleChoice
{
    public record ScqEditModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public IEnumerable<AnswerOptionEditModel> AnswerOptions { get; set; }
    }
}
