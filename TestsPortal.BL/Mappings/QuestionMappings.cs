using AutoMapper;
using TestsDelivery.UserModels.Questions.BaseQuestion;
using TestsDelivery.UserModels.Questions.Essay;
using TestsDelivery.UserModels.Questions.MultipleChoice;
using TestsDelivery.UserModels.Questions.SingleChoice;
using TestsPortal.DAL.Models.Questions;
using TestsPortal.Domain.Questions;
using DALShortQuestion = TestsPortal.DAL.Models.Questions.ShortQuestion;
using DomainShortQuestion = TestsPortal.Domain.Questions.ShortQuestion;

namespace TestsPortal.BL.Mappings
{
    public class QuestionMappings : Profile
    {
        public QuestionMappings()
        {
            CreateMap<QuestionReadModel, QuestionBase>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id))
                .Include<QuestionWithOptionsReadModel, QuestionWithOptionsBase>()
                .Include<EssayReadModel, QuestionBase>()
                .Include<ScqReadModel, QuestionBase>()
                .Include<McqReadModel, QuestionBase>()
                .ReverseMap();

            CreateMap<QuestionBase, EssayQuestion>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id));
            CreateMap<QuestionBase, SingleChoiceQuestion>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id));
            CreateMap<QuestionBase, MultipleChoiceQuestion>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id));
            CreateMap<QuestionWithOptionsReadModel, QuestionWithOptionsBase>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id));
            CreateMap<EssayReadModel, QuestionBase>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id));
            CreateMap<ScqReadModel, QuestionBase>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id));
            CreateMap<McqReadModel, QuestionBase>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id));

            CreateMap<ScqDetailedModel, SingleChoiceQuestion>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id));
            CreateMap<McqDetailedModel, MultipleChoiceQuestion>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id));
            CreateMap<EssayDetailedModel, EssayQuestion>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id));

            CreateMap<SingleChoiceQuestion, Question>()
                .ForMember(x => x.Subject, x => x.Ignore())
                .ReverseMap();
            CreateMap<MultipleChoiceQuestion, Question>()
                .ForMember(x => x.Subject, x => x.Ignore())
                .ReverseMap();
            CreateMap<EssayQuestion, Question>()
                .ForMember(x => x.Subject, x => x.Ignore())
                .ReverseMap();

            CreateMap<QuestionBase, QuestionReadModel>();
            CreateMap<QuestionBase, EssayReadModel>();
            CreateMap<QuestionBase, McqReadModel>();
            CreateMap<QuestionBase, ScqReadModel>();
            CreateMap<QuestionWithOptionsBase, QuestionWithOptionsReadModel>();

            CreateMap<SingleChoiceQuestion, ScqReadModel>();
            CreateMap<MultipleChoiceQuestion, McqReadModel>();
            CreateMap<EssayQuestion, EssayReadModel>();

            CreateMap<Question, DALShortQuestion>();
            CreateMap<DALShortQuestion, DomainShortQuestion>();
            CreateMap<DomainShortQuestion, QuestionInListModel>();

            CreateMap<Question, QuestionBase>();
        }
    }
}
