using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsPortal.Domain.AnsweredQuestions.Answers;

namespace TestsPortal.BL.Services.Questions.TypedQuestion
{
    public class EssayService : TypedQuestionService<EssayAnswer>, IEssayService
    {
        public EssayService(IMapper mapper)
            : base(mapper)
        {
        }
    }
}
