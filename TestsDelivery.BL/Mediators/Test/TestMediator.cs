using AutoMapper;
using TestsDelivery.UserModels.Test;
using TestsDelivery.BL.Services.Test;
using TestsDelivery.Domain.Lists;
using TestsDelivery.UserModels.ListFilters;

namespace TestsDelivery.BL.Mediators.Test
{
    public class TestMediator : ITestMediator
    {
        private readonly IMapper _mapper;
        private readonly ITestService _service;

        public TestMediator(ITestService service, IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }

        public TestReadModel CreateTest(TestCreateModel model)
        {
            var test = _mapper.Map<Domain.Test.Test>(model);
            return _mapper.Map<TestReadModel>(_service.CreateTest(test));
        }

        public TestReadModel GetTest(long id)
        {
            return _mapper.Map<TestReadModel>(_service.GetTest(id));
        }

        public void EditTest(TestEditModel model)
        {
            var test = _mapper.Map<Domain.Test.Test>(model);
            _service.EditTest(test);
        }

        public TestsListModel GetList(ListFilterModel model)
        {
            var filter = _mapper.Map<ListFilter>(model);
            return _mapper.Map<TestsListModel>(_service.GetList(filter));
        }
    }
}
