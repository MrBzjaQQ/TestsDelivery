using AutoMapper;
using TestsPortal.DAL.Models.Questions;
using TestsPortal.DAL.Repositories.Answers;
using TestsPortal.Domain.AnsweredQuestions.Answers;

namespace TestsPortal.BL.Services.Questions.TypedQuestion
{
    public class MultipleChoiceService : IMultipleChoiceService
    {
        private IMapper _mapper;
        private IChoiceAnswersRepository _answersRepository;

        public MultipleChoiceService(IChoiceAnswersRepository answersRepository, IMapper mapper)
        {
            _mapper = mapper;
            _answersRepository = answersRepository;
        }

        public void PostAnswer(MultipleChoiceAnswer answer)
        {
            var dalAnswers = MapAnswerToData(answer);
            _answersRepository.Create(dalAnswers);
        }

        private IEnumerable<ChoiceAnswer> MapAnswerToData(MultipleChoiceAnswer answer)
        {
            return answer.SelectedAnswerIds.Select(id => new ChoiceAnswer
            {
                AnswerId = id,
                ScheduledTestId = answer.ScheduledTestId,
                CandidateId = answer.CandidateId,
                QuestionId = answer.QuestionId
            });
        }
    }
}
