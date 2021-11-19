using AutoMapper;
using TestsDelivery.DAL.Repositories.AnswerOptions;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Services.Questions.SingleChoice
{
    public class ScqService : BaseQuestionWithOptionsService<SingleChoiceQuestion>, IScqService
    {
        public ScqService(
            IQuestionsRepository questionsRepository,
            IAnswerOptionsRepository answerOptionsRepository,
            IMapper mapper) : base(questionsRepository, answerOptionsRepository, mapper)
        {
        }
    }
}
