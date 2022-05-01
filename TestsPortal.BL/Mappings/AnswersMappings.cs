using AutoMapper;
using TestsDelivery.UserModels.AnsweredQuestions.AnswerModels;
using TestsPortal.Domain.AnsweredQuestions.Answers;

namespace TestsPortal.BL.Mappings
{
    public class AnswersMappings : Profile
    {
        public AnswersMappings()
        {
            CreateMap<EssayAnswerModel, EssayAnswer>();
            CreateMap<SingleChoiceAnswerModel, SingleChoiceAnswer>();
            CreateMap<MultipleChoiceAnswerModel, MultipleChoiceAnswer>();
        }
    }
}
