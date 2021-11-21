using TestsDelivery.BL.Models.Test;

namespace TestsDelivery.BL.Mediators.Test
{
    public interface ITestMediator
    {
        TestReadModel CreateTest(TestCreateModel model);

        TestReadModel GetTest(long id);

        void EditTest(TestEditModel model);
    }
}
