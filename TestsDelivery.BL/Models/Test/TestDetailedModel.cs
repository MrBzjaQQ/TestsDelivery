using System.Collections.Generic;
using TestsDelivery.BL.Models.Subject;

namespace TestsDelivery.BL.Models.Test
{
    public class TestDetailedModel
    {
        public string Name { get; set; }

        public IEnumerable<SubjectInTestModel> Subjects { get; set; }
    }
}
