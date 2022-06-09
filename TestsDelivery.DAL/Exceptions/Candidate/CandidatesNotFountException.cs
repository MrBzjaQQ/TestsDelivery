using System.Collections.Generic;

namespace TestsDelivery.DAL.Exceptions.Candidate
{
    public class CandidatesNotFountException : CandidateException
    {
        public CandidatesNotFountException(IEnumerable<long> missingIds)
            : base($"Candidates are not found by the following ids: {string.Join(',', missingIds)}")
        {
        }
    }
}
