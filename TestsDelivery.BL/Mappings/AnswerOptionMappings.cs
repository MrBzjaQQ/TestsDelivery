using AutoMapper;
using TestsDelivery.UserModels.AnswerOptions;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Mappings
{
    public class AnswerOptionMappings : Profile
    {
        public AnswerOptionMappings()
        {
            CreateMap<AnswerOptionCreateModel, AnswerOption>();
            CreateMap<AnswerOptionEditModel, AnswerOption>();
            CreateMap<AnswerOption, AnswerOptionReadModel>();
            CreateMap<AnswerOption, AnswerOptionUnverified>();

            CreateMap<AnswerOption, DAL.Models.Questions.AnswerOption>();
            CreateMap<DAL.Models.Questions.AnswerOption, AnswerOption>();
        }
    }
}
