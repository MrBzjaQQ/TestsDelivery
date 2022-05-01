using AutoMapper;
using TestsPortal.Domain.AnsweredQuestions.Answers;

namespace TestsPortal.BL.Services.Questions.TypedQuestion
{
    public class SingleChoiceService : TypedQuestionService<SingleChoiceAnswer>, ISingleChoiceService
    {
        public SingleChoiceService(IMapper mapper)
            : base(mapper)
        {
        }
    }
}
