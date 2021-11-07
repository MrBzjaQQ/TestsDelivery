using AutoMapper;
using TestsDelivery.BL.Models.Questions.AnswerOptions;
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

            CreateMap<AnswerOption, DAL.Models.Questions.AnswerOption>();
            CreateMap<DAL.Models.Questions.AnswerOption, AnswerOption>();
        }
    }
}
