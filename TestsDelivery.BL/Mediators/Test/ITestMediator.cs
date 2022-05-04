using System.Collections.Generic;
using TestsDelivery.UserModels.Lists;
using TestsDelivery.UserModels.Test;

namespace TestsDelivery.BL.Mediators.Test
{
    public interface ITestMediator
    {
        TestReadModel CreateTest(TestCreateModel model);

        TestReadModel GetTest(long id);

        void EditTest(TestEditModel model);

        IEnumerable<TestInListModel> GetList(ListModel model);
    }
}
