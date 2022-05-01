using AutoMapper;
using TestsPortal.Domain.AnsweredQuestions.Answers;

namespace TestsPortal.BL.Services.Questions.TypedQuestion
{
    public class MultipleChoiceService : TypedQuestionService<MultipleChoiceAnswer>, IMultipleChoiceService
    {
        public MultipleChoiceService(IMapper mapper)
            : base(mapper)
        {
        }
    }
}
