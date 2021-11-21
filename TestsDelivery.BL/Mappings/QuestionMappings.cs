using AutoMapper;
using TestsDelivery.BL.Models.Questions.BaseQuestion;
using TestsDelivery.BL.Models.Questions.Essay;
using TestsDelivery.BL.Models.Questions.MultipleChoice;
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
            CreateMap<ScqEditModel, SingleChoiceQuestion>()
                .ForMember(x => x.Type, x => x.MapFrom(y => QuestionType.SingleChoice));
            CreateMap<ScqCreateModel, SingleChoiceQuestion>()
                .ForMember(x => x.Subject, x => x.MapFrom(y => new Subject { Id = y.SubjectId }))
                .ForMember(x => x.Type, x => x.MapFrom(y => QuestionType.SingleChoice));

            CreateMap<SingleChoiceQuestion, Question>()
                .ForMember(x => x.SubjectId, x => x.MapFrom(y => y.Subject.Id))
                .ForMember(x => x.Subject, x => x.Ignore())
                .ForMember(x => x.Type, x => x.MapFrom(y => y.Type));

            CreateMap<Question, SingleChoiceQuestion>()
                .ForMember(x => x.Type, x => x.MapFrom(y => y.Type))
                .ForMember(x => x.Subject, x => x.MapFrom(y => y.Subject));

            CreateMap<MultipleChoiceQuestion, McqReadModel>()
                .ForMember(x => x.Subject, x => x.MapFrom(y => y.Subject));
            CreateMap<McqEditModel, MultipleChoiceQuestion>()
                .ForMember(x => x.Type, x => x.MapFrom(y => QuestionType.MultipleChoice)); ;
            CreateMap<McqCreateModel, MultipleChoiceQuestion>()
                .ForMember(x => x.Subject, x => x.MapFrom(y => new Subject { Id = y.SubjectId }))
                .ForMember(x => x.Type, x => x.MapFrom(y => QuestionType.MultipleChoice)); ;

            CreateMap<MultipleChoiceQuestion, Question>()
                .ForMember(x => x.SubjectId, x => x.MapFrom(y => y.Subject.Id))
                .ForMember(x => x.Subject, x => x.Ignore())
                .ForMember(x => x.Type, x => x.MapFrom(y => y.Type));

            CreateMap<Question, MultipleChoiceQuestion>()
                .ForMember(x => x.Type, x => x.MapFrom(y => y.Type))
                .ForMember(x => x.Subject, x => x.MapFrom(y => y.Subject));

            CreateMap<EssayQuestion, EssayReadModel>()
                .ForMember(x => x.Subject, x => x.MapFrom(y => y.Subject));
            CreateMap<EssayEditModel, EssayQuestion>()
                .ForMember(x => x.Type, x => x.MapFrom(y => QuestionType.Essay));
            CreateMap<EssayCreateModel, EssayQuestion>()
                .ForMember(x => x.Subject, x => x.MapFrom(y => new Subject { Id = y.SubjectId }))
                .ForMember(x => x.Type, x => x.MapFrom(y => QuestionType.Essay));

            CreateMap<EssayQuestion, Question>()
                .ForMember(x => x.SubjectId, x => x.MapFrom(y => y.Subject.Id))
                .ForMember(x => x.Subject, x => x.Ignore())
                .ForMember(x => x.Type, x => x.MapFrom(y => y.Type));

            CreateMap<Question, EssayQuestion>()
                .ForMember(x => x.Type, x => x.MapFrom(y => y.Type))
                .ForMember(x => x.Subject, x => x.MapFrom(y => y.Subject));

            CreateMap<QuestionBase, Question>()
                .ForMember(x => x.SubjectId, x => x.MapFrom(y => y.Subject.Id))
                .ForMember(x => x.Subject, x => x.Ignore())
                .ForMember(x => x.Type, x => x.MapFrom(y => y.Type));
            CreateMap<Question, QuestionBase>()
                .ForMember(x => x.Type, x => x.MapFrom(y => y.Type))
                .ForMember(x => x.Subject, x => x.MapFrom(y => y.Subject));
            CreateMap<QuestionBase, BaseQuestionReadModel>();
        }
    }
}
