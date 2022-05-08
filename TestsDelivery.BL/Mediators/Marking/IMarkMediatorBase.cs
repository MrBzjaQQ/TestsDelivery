using TestsDelivery.UserModels.Marking.FilterModels;

namespace TestsDelivery.BL.Mediators.Marking
{
    public interface IMarkMediatorBase<in TMarkCreateModel, out TMarkReadModel>
    {
        TMarkReadModel Create(TMarkCreateModel createModel);

        TMarkReadModel GetMark(GetMarkModel markModel);
    }
}
