using AutoMapper;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Services.Questions.Essay
{
    public class EssayService : BaseQuestionService<EssayQuestion>, IEssayService
    {
        public EssayService(IQuestionsRepository repository, IMapper mapper)
            :base(repository, mapper, QuestionType.Essay)
        {
        }
    }
}
