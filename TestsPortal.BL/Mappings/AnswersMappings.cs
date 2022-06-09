using AutoMapper;
using TestsDelivery.UserModels.AnsweredQuestions.AnswerModels;
using TestsPortal.DAL.Models.Questions;
using TestsPortal.Domain.AnsweredQuestions.Answers;

namespace TestsPortal.BL.Mappings
{
    public class AnswersMappings : Profile
    {
        public AnswersMappings()
        {
            CreateMap<EssayAnswerCreateModel, EssayAnswer>();
            CreateMap<SingleChoiceAnswerCreateModel, SingleChoiceAnswer>();
            CreateMap<MultipleChoiceAnswerCreateModel, MultipleChoiceAnswer>();

            CreateMap<EssayAnswer, TextAnswer>();
            CreateMap<SingleChoiceAnswerCreateModel, ChoiceAnswer>();

            CreateMap<EssayAnswer, EssayAnswerReadModel>();
            CreateMap<SingleChoiceAnswer, SingleChoiceAnswerReadModel>();
            CreateMap<MultipleChoiceAnswerCreateModel, MultipleChoiceAnswerReadModel>();
        }
    }
}
