using AutoMapper;
using TestsDelivery.DAL.Models.Marking;
using TestsDelivery.DAL.Repositories.Marking;
using TestsDelivery.Domain.Marking;
using TestsDelivery.Domain.Marking.FilterModels;

namespace TestsDelivery.BL.Services.Marking
{
    public class MarkServiceBase<TDomainModel, TDataModel> : IMarkServiceBase<TDomainModel>
        where TDomainModel : MarkedQuestionBase
        where TDataModel : MarkBase
    {
        protected IMarkingRepositoryBase<TDataModel> Repository { get; set; }
        protected IMapper Mapper { get; set; }

        public MarkServiceBase(IMarkingRepositoryBase<TDataModel> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public TDomainModel Create(TDomainModel model)
        {
            var dataModel = Mapper.Map<TDataModel>(model);
            Repository.Create(dataModel);
            return Mapper.Map<TDomainModel>(dataModel);
        }

        public TDomainModel GetMark(GetMarkFilter markFilter)
        {
            return Mapper.Map<TDomainModel>(Repository.GetMark(
                markFilter.QuestionId,
                markFilter.TestId));
        }
    }
}
