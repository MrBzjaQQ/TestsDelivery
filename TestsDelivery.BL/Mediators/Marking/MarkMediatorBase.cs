using AutoMapper;
using TestsDelivery.BL.Services.Marking;
using TestsDelivery.Domain.Marking.FilterModels;
using TestsDelivery.UserModels.Marking.FilterModels;

namespace TestsDelivery.BL.Mediators.Marking
{
    public abstract class MarkMediatorBase<TMarkCreateModel, TMarkReadModel, TMarkDomainModel> : IMarkMediatorBase<TMarkCreateModel, TMarkReadModel>
    {
        protected readonly IMarkServiceBase<TMarkDomainModel> MarkService;
        protected readonly IMapper Mapper;

        public MarkMediatorBase(IMarkServiceBase<TMarkDomainModel> markService, IMapper mapper)
        {
            MarkService = markService;
            Mapper = mapper;
        }

        public TMarkReadModel Create(TMarkCreateModel createModel)
        {
            var domainModel = Mapper.Map<TMarkDomainModel>(createModel);
            return Mapper.Map<TMarkReadModel>(MarkService.Create(domainModel));
        }

        public TMarkReadModel GetMark(GetMarkModel markModel)
        {
            var markFilter = Mapper.Map<GetMarkFilter>(markModel);
            return Mapper.Map<TMarkReadModel>(MarkService.GetMark(markFilter));
        }
    }
}
