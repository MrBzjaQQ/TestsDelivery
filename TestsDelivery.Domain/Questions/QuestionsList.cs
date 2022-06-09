using System.Collections.Generic;

namespace TestsDelivery.Domain.Questions
{
    public record QuestionsList
    {
        public IEnumerable<ShortQuestion> Questions { get; set; }

        public int TotalCount { get; set; }
    }
}
