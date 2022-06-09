using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.Candidate
{
    public class CandidateRepository : BaseRepository<TestsDeliveryContext, Models.Candidate.Candidate>, ICandidateRepository
    {
        public CandidateRepository(TestsDeliveryContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public IEnumerable<Models.Candidate.Candidate> GetCandidates(IEnumerable<long> ids)
        {
            return Context.Candidates.Where(x => ids.Contains(x.Id)).ToList();
        }
    }
}
