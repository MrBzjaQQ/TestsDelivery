using TestsDelivery.Domain.Marking.FilterModels;

namespace TestsDelivery.BL.Services.Marking
{
    public interface IMarkServiceBase<TDomainModel>
    {
        TDomainModel Create(TDomainModel model);

        TDomainModel GetMark(GetMarkFilter markFilter);
    }
}
