﻿using AutoMapper;
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
                .ForMember(x => x.Questions, x => x.MapFrom<QuestionsClientToDomainResolver>());

            CreateMap<Test, DAL.Models.Tests.Test>();
            CreateMap<DAL.Models.Tests.Test, Test>();
        }

        private class QuestionsClientToDomainResolver : IValueResolver<TestDetailedModel, Test, IEnumerable<QuestionBase>>
        {
            public IEnumerable<QuestionBase> Resolve(TestDetailedModel source, Test destination, IEnumerable<QuestionBase> destMember, ResolutionContext context)
            {
                var dest = destination?.Questions != null ? new List<QuestionBase>(destination.Questions) : new List<QuestionBase>();
                foreach (var question in source.Questions)
                {
                    var type = question.GetType();

                    if (type.IsAssignableTo(typeof(ScqDetailedModel)))
                        dest.Add(MapQuestion<ScqDetailedModel, SingleChoiceQuestion>(question, context.Mapper));
                    else if (type.IsAssignableTo(typeof(McqDetailedModel)))
                        dest.Add(MapQuestion<McqDetailedModel, MultipleChoiceQuestion>(question, context.Mapper));
                    else if (type.IsAssignableTo(typeof(EssayDetailedModel)))
                        dest.Add(MapQuestion<EssayDetailedModel, EssayQuestion>(question, context.Mapper));
                    else
                        throw new ArgumentException("Unknown question type when mapping", nameof(question));
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
