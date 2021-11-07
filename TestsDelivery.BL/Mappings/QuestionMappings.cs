using AutoMapper;
using TestsDelivery.BL.Models.Questions.SingleChoice;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.Domain.Questions;
using TestsDelivery.Domain.Subject;

namespace TestsDelivery.BL.Mappings
{
    public class QuestionMappings : Profile
    {
        public QuestionMappings()
        {
            CreateMap<SingleChoiceQuestion, ScqReadModel>()
                .ForMember(x => x.Subject, x => x.MapFrom(y => y.Subject));
            CreateMap<ScqEditModel, SingleChoiceQuestion>();
            CreateMap<ScqCreateModel, SingleChoiceQuestion>()
                .ForMember(x => x.Subject, x => x.MapFrom(y => new Subject { Id = y.SubjectId }));

            CreateMap<SingleChoiceQuestion, Question>()
                .ForMember(x => x.SubjectId, x => x.MapFrom(y => y.Subject.Id))
                .ForMember(x => x.Subject, x => x.Ignore());

            CreateMap<Question, SingleChoiceQuestion>();
        }
    }
}
