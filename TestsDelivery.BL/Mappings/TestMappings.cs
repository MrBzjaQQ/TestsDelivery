using System.Linq;
using AutoMapper;
using TestsDelivery.UserModels.Test;
using TestsDelivery.Domain.Questions;
using TestsDelivery.Domain.Test;

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
                .ForMember(x => x.Questions, x => x.MapFrom(y => y.QuestionIds.Distinct().Select(id => new QuestionBase { Id = id }))); ;

            CreateMap<Test, DAL.Models.Test.Test>();
            CreateMap<DAL.Models.Test.Test, Test>();
        }
    }
}
