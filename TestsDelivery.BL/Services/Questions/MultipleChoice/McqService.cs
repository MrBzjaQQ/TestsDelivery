using AutoMapper;
using TestsDelivery.DAL.Repositories.AnswerOptions;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Services.Questions.MultipleChoice
{
    public class McqService : BaseQuestionWithOptionsService<MultipleChoiceQuestion>, IMcqService
    {
        public McqService(
            IQuestionsRepository questionsRepository,
            IAnswerOptionsRepository answerOptionsRepository,
            IMapper mapper) : base(questionsRepository, answerOptionsRepository, mapper)
        {
        }
    }
}
