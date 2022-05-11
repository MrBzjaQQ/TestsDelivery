using System.Collections.Generic;
using TestsDelivery.Domain.Lists;
using TestsDelivery.Domain.Test;
using TestDomainModel = TestsDelivery.Domain.Test.Test;

namespace TestsDelivery.BL.Services.Test
{
    public interface ITestService
    {
        TestDomainModel CreateTest(TestDomainModel test);

        TestDomainModel GetTest(long id);

        TestDomainModel GetFullTest(long id);

        void EditTest(TestDomainModel test);

        TestsList GetList(ListFilter filter);
    }
}
