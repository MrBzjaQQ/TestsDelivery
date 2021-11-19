using AutoMapper;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Services.Questions.Essay
{
    public class EssayService : BaseQuestionService<EssayQuestion, Question>, IEssayService
    {
        public EssayService(IQuestionsRepository repository, IMapper mapper)
            :base(repository, mapper)
        {
        }
    }
}
