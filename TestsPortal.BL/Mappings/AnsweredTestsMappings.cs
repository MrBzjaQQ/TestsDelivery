using AutoMapper;
using TestsDelivery.UserModels.AnsweredQuestions.AnswerModels;
using TestsPortal.DAL.Models.ScheduledTests;
using TestsPortal.Domain.AnsweredTests;
using TestsPortal.Domain.Questions.AnsweredQuestions;
using AnsweredTestReadModel = TestsDelivery.UserModels.AnsweredTests.AnsweredTestCreateModel;

namespace TestsPortal.BL.Mappings
{
    public class AnsweredTestsMappings : Profile
    {
        public AnsweredTestsMappings()
        {
            CreateMap<ScheduledTestInstance, AnsweredTest>()
                .ForMember(x => x.Id, x => x.MapFrom(y => y.ScheduledTest.Id))
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.ScheduledTest.OriginalId))
                .ForMember(x => x.CandidateId, x => x.MapFrom(y => y.CandidateId));

            CreateMap<AnsweredTest, AnsweredTestReadModel>()
                .ForMember(x => x.ScheduledTestId, x => x.MapFrom(y => y.OriginalId))
                .ForMember(x => x.Answers, x => x.MapFrom(y => y.AnsweredQuestions));
        }

        private class AnsweredTestDomainToUserModelResolver : IValueResolver<AnsweredTest, AnsweredTestReadModel, IEnumerable<AnswerReadModelBase>>
        {
            public IEnumerable<AnswerReadModelBase> Resolve(AnsweredTest source, AnsweredTestReadModel destination, IEnumerable<AnswerReadModelBase> destMember, ResolutionContext context)
            {
                var dest = destination?.Answers == null ? new List<AnswerReadModelBase>() : new List<AnswerReadModelBase>(destination.Answers);
                foreach (var answer in source.AnsweredQuestions)
                {
                    var type = answer.GetType();

                    if (type.IsAssignableTo(typeof(AnsweredEssay)))
                        dest.Add(MapAnswer<AnsweredEssay, EssayAnswerReadModel>(answer, context.Mapper));
                    else if (type.IsAssignableTo(typeof(AnsweredSingleChoice)))
                        dest.Add(MapAnswer<AnsweredSingleChoice, SingleChoiceAnswerReadModel>(answer, context.Mapper));
                    else if (type.IsAssignableTo(typeof(AnsweredMultipleChoice)))
                        dest.Add(MapAnswer<AnsweredMultipleChoice, MultipleChoiceAnswerReadModel>(answer, context.Mapper));
                    else
                        throw new ArgumentException("Unknown answer type when mapping", nameof(answer));
                    return dest;
                }
                return dest;
            }

            private AnswerReadModelBase MapAnswer<TSource, TDestination>(AnsweredQuestionBase answer, IRuntimeMapper mapper)
                where TSource : AnsweredQuestionBase
                where TDestination : AnswerReadModelBase
            {
                var model = answer as TSource;
                if (answer == null)
                    throw new ArgumentNullException(nameof(model));

                return mapper.Map<TDestination>(model);
            }
        }
    }
}
