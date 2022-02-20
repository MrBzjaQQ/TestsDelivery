using AutoMapper;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Services.Questions
{
    public abstract class BaseQuestionService<TDomainQuestion> : IBaseQuestionService<TDomainQuestion>
        where TDomainQuestion : QuestionBase
    {
        protected readonly IQuestionsRepository QuestionsRepository;
        protected readonly IMapper Mapper;
        protected readonly QuestionType QuestionType;

        protected BaseQuestionService(IQuestionsRepository repository, IMapper mapper, QuestionType questionType)
        {
            QuestionsRepository = repository;
            Mapper = mapper;
            QuestionType = questionType;
        }

        public virtual TDomainQuestion CreateQuestion(TDomainQuestion question)
        {
            var questionData = Mapper.Map<Question>(question);
            QuestionsRepository.CreateQuestion(questionData);

            var questionResult = Mapper.Map<TDomainQuestion>(QuestionsRepository.GetQuestion(questionData.Id));
            return questionResult;
        }

        public virtual void DeleteQuestion(long id)
        {
            QuestionsRepository.DeleteQuestion(id, (short)QuestionType);
        }

        public virtual void EditQuestion(TDomainQuestion question)
        {
            var questionData = Mapper.Map<Question>(question);
            QuestionsRepository.EditQuestion(questionData);
        }

        public virtual TDomainQuestion GetQuestion(long id)
        {
            return Mapper.Map<TDomainQuestion>(QuestionsRepository.GetQuestion(id, (short)QuestionType));
        }
    }
}
