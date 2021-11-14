using AutoMapper;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Services.Questions.Essay
{
    public class EssayService : IEssayService
    {
        private readonly IQuestionsRepository _repository;
        private readonly IMapper _mapper;

        public EssayService(IQuestionsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public EssayQuestion CreateQuestion(EssayQuestion question)
        {
            var questionData = _mapper.Map<Question>(question);
            _repository.CreateQuestion(questionData);

            var questionResult = _mapper.Map<EssayQuestion>(question);
            return questionResult;
        }

        public void EditQuestion(EssayQuestion question)
        {
            var questionData = _mapper.Map<Question>(question);
            _repository.EditQuestion(questionData);
        }

        public EssayQuestion GetQuestion(long id)
        {
            return _mapper.Map<EssayQuestion>(_repository.GetQuestion(id));
        }
    }
}
