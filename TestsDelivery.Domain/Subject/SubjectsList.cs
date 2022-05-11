using System.Collections.Generic;

namespace TestsDelivery.Domain.Subject
{
    public record SubjectsList
    {
        public IEnumerable<SubjectInListDto> Subjects { get; set; }

        public int TotalCount { get; set; }
    }
}
