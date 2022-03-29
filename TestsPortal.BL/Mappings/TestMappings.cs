using AutoMapper;
using TestsDelivery.UserModels.Questions.BaseQuestion;
using TestsDelivery.UserModels.Questions.Essay;
using TestsDelivery.UserModels.Questions.MultipleChoice;
using TestsDelivery.UserModels.Questions.SingleChoice;
using TestsDelivery.UserModels.Test;
using TestsPortal.Domain.Questions;
using TestsPortal.Domain.Tests;

namespace TestsPortal.BL.Mappings
{
    public class TestMappings : Profile
    {
        public TestMappings()
        {
            CreateMap<TestDetailedModel, Test>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.Questions, x => x.MapFrom<QuestionsResolver>());
        }

        private class QuestionsResolver : IValueResolver<TestDetailedModel, Test, IEnumerable<QuestionBase>>
        {
            public IEnumerable<QuestionBase> Resolve(TestDetailedModel source, Test destination, IEnumerable<QuestionBase> destMember, ResolutionContext context)
            {
                var dest = destination?.Questions != null ? new List<QuestionBase>(destination.Questions) : new List<QuestionBase>();
                foreach (var question in source.Questions)
                {
                    var type = question.GetType();

                    if (type.IsInstanceOfType(typeof(ScqReadModel)))
                        dest.Add(MapQuestion<ScqReadModel, SingleChoiceQuestion>(question, context.Mapper));
                    else if (type.IsInstanceOfType(typeof(McqReadModel)))
                        dest.Add(MapQuestion<McqReadModel, MultipleChoiceQuestion>(question, context.Mapper));
                    else if (type.IsInstanceOfType(typeof(EssayReadModel)))
                        dest.Add(MapQuestion<EssayReadModel, EssayQuestion>(question, context.Mapper));
                }

                return dest;
            }

            private QuestionBase MapQuestion<TSource, TDestination>(QuestionReadModel model, IRuntimeMapper mapper)
                where TSource : QuestionReadModel
                where TDestination : QuestionBase
            {
                var typedQuestion = model as TSource;
                if (typedQuestion == null)
                    throw new ArgumentException(nameof(model));

                return mapper.Map<TDestination>(typedQuestion);
            }
        }
    }
}
