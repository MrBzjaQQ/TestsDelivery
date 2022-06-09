using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsDelivery.UserModels.AnsweredQuestions.AnswerModels
{
    public record MultipleChoiceAnswerReadModel : AnswerReadModelBase
    {
        public IEnumerable<long> SelectedAnswerIds { get; set; }
    }
}
