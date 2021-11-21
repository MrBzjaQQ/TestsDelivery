using System.Collections.Generic;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.Domain.Test
{
    public record Test
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<QuestionBase> Questions { get; set; }
    }
}
