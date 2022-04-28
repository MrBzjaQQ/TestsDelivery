using System;
using System.Collections.Generic;
using System.Linq;
using TestsDelivery.DAL.Data;

namespace TestsDelivery.DAL.Repositories.Candidate
{
    public class CandidateRepository : BaseRepository<Models.Candidate.Candidate>, ICandidateRepository
    {
        public CandidateRepository(TestsDeliveryContext context)
            : base(context)
        {
        }

        public IEnumerable<Models.Candidate.Candidate> GetCandidates(IEnumerable<long> ids)
        {
            return Context.Candidates.Where(x => ids.Contains(x.Id)).ToList();
        }
    }
}
