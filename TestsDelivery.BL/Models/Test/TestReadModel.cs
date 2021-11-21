using System.Collections.Generic;
using TestsDelivery.BL.Models.Questions.BaseQuestion;

namespace TestsDelivery.BL.Models.Test
{
    public record TestReadModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<BaseQuestionReadModel> Questions { get; set; }
    }
}
