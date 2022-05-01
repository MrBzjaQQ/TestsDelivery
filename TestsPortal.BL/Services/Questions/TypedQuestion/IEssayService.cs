using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsPortal.Domain.AnsweredQuestions.Answers;

namespace TestsPortal.BL.Services.Questions.TypedQuestion
{
    public interface IEssayService : ITypedQuestionService<EssayAnswer>
    {
    }
}
