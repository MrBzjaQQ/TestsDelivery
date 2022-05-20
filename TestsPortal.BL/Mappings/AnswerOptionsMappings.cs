using AutoMapper;
using TestsDelivery.UserModels.AnswerOptions;
using AnswerOptionDomain = TestsPortal.Domain.Questions.AnswerOption;
using AnswerOptionData = TestsPortal.DAL.Models.Questions.AnswerOption;

namespace TestsPortal.BL.Mappings
{
    public class AnswerOptionsMappings : Profile
    {
        public AnswerOptionsMappings()
        {
            CreateMap<AnswerOptionUnverified, AnswerOptionDomain>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id));
            CreateMap<AnswerOptionDomain, AnswerOptionData>()
                .ReverseMap();
            CreateMap<AnswerOptionDomain, AnswerOptionReadModel>();
        }
    }
}
