using System.Linq;
using AutoMapper;
using TestsDelivery.UserModels.Test;
using TestsDelivery.Domain.Questions;
using TestsDelivery.Domain.Test;
using System.Collections.Generic;
using TestsDelivery.UserModels.Questions.BaseQuestion;
using TestsDelivery.UserModels.Questions.SingleChoice;
using TestsDelivery.UserModels.Questions.MultipleChoice;
using TestsDelivery.UserModels.Questions.Essay;
using System;

namespace TestsDelivery.BL.Mappings
{
    public class TestMappings : Profile
    {
        public TestMappings()
        {
            CreateMap<TestCreateModel, Test>()
                .ForMember(x => x.Questions, x => x.MapFrom(y => y.QuestionIds.Distinct().Select(id => new QuestionBase { Id = id })));
            CreateMap<Test, TestReadModel>();
            CreateMap<TestReadModel, Test>();
            CreateMap<TestEditModel, Test>()
                .ForMember(x => x.Questions, x => x.MapFrom(y => y.QuestionIds.Distinct().Select(id => new QuestionBase { Id = id })));

            CreateMap<Test, DAL.Models.Test.Test>();
            CreateMap<DAL.Models.Test.Test, Test>();
            CreateMap<Test, TestDetailedModel>()
                .ForMember(x => x.Questions, x => x.MapFrom<QuestionsDomainToClientResolver>());

            CreateMap<TestInListModel, TestInListDto>()
                .ReverseMap();
        }

        private class QuestionsDomainToClientResolver : IValueResolver<Test, TestDetailedModel, IEnumerable<QuestionDetailedReadModel>>
        {
            public IEnumerable<QuestionDetailedReadModel> Resolve(Test source, TestDetailedModel destination, IEnumerable<QuestionDetailedReadModel> destMember, ResolutionContext context)
            {
                var dest = destination?.Questions != null ? new List<QuestionDetailedReadModel>(destination.Questions) : new List<QuestionDetailedReadModel>();
                foreach (var question in source.Questions)
                {
                    var type = question.GetType();

                    if (type.IsAssignableFrom(typeof(SingleChoiceQuestion)))
                        dest.Add(MapQuestion<SingleChoiceQuestion, ScqDetailedModel>(question, context.Mapper));
                    else if (type.IsAssignableFrom(typeof(MultipleChoiceQuestion)))
                        dest.Add(MapQuestion<MultipleChoiceQuestion, McqDetailedModel>(question, context.Mapper));
                    else if (type.IsAssignableFrom(typeof(EssayQuestion)))
                        dest.Add(MapQuestion<EssayQuestion, EssayDetailedModel>(question, context.Mapper));
                    else
                        throw new ArgumentException("Unknown question type when mapping", nameof(question));
                }

                return dest;
            }

            private QuestionDetailedReadModel MapQuestion<TSource, TDestination>(QuestionBase model, IRuntimeMapper mapper)
                where TSource : QuestionBase
                where TDestination : QuestionDetailedReadModel
            {
                var typedQuestion = model as TSource;
                if (typedQuestion == null)
                    throw new ArgumentException(nameof(model));

                return mapper.Map<TDestination>(typedQuestion);
            }
        }
    }
}
