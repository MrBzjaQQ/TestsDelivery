using System.Collections.Generic;

namespace TestsDelivery.Domain.Test
{
    public record TestsList
    {
        public IEnumerable<TestInListDto> Tests { get; set; }

        public int TotalCount { get; set; }
    }
}
