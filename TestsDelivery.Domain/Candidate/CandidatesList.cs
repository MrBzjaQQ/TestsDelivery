using System.Collections.Generic;

namespace TestsDelivery.Domain.Candidate
{
    public record CandidatesList
    {
        public IEnumerable<Candidate> Candidates { get; set; }

        public int TotalCount { get; set; }
    }
}
