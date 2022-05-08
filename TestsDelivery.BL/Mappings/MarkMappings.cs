using AutoMapper;
using TestsDelivery.UserModels.Marking.Questions;
using TestsDelivery.UserModels.Marking.FilterModels;
using TestsDelivery.Domain.Marking;
using TestsDelivery.DAL.Models.Marking;
using TestsDelivery.Domain.Marking.FilterModels;

namespace TestsDelivery.BL.Mappings
{
    public class MarkMappings : Profile
    {
        public MarkMappings()
        {
            CreateMap<EssayMarkCreateModel, MarkedEssay>()
                .ForMember(x => x.TestInstanceId, x => x.MapFrom(y => y.TestId));
            CreateMap<ScqMarkCreateModel, MarkedSingleChoice>()
                .ForMember(x => x.TestInstanceId, x => x.MapFrom(y => y.TestId));
            CreateMap<McqMarkCreateModel, MarkedMultipleChoice>()
                .ForMember(x => x.TestInstanceId, x => x.MapFrom(y => y.TestId));

            CreateMap<MarkedEssay, EssayMark>()
                .ReverseMap();
            CreateMap<MarkedSingleChoice, ChoiceMark>()
                .ReverseMap();
            CreateMap<MarkedMultipleChoice, ChoiceMark>()
                .ReverseMap();

            CreateMap<EssayMarkReadModel, MarkedEssay>();
            CreateMap<ScqMarkReadModel, MarkedSingleChoice>();
            CreateMap<McqMarkReadModel, MarkedMultipleChoice>();

            CreateMap<GetMarkModel, GetMarkFilter>();
        }
    }
}
