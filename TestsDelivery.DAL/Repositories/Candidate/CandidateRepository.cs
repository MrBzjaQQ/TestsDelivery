using System;
using System.Collections.Generic;
using System.Linq;
using TestsDelivery.DAL.Data;

namespace TestsDelivery.DAL.Repositories.Candidate
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly TestsDeliveryContext _context;

        public CandidateRepository(TestsDeliveryContext context)
        {
            _context = context;
        }

        public Models.Candidate.Candidate GetCandidate(long id)
        {
            return _context.Candidates.Single(c => c.Id == id);
        }

        public IEnumerable<Models.Candidate.Candidate> GetCandidates(IEnumerable<long> ids)
        {
            return _context.Candidates.Where(x => ids.Contains(x.Id)).ToList();
        }

        public void CreateCandidate(Models.Candidate.Candidate candidate)
        {
            _context.Candidates.Add(candidate);
            _context.SaveChanges();
        }

        public void EditCandidate(Models.Candidate.Candidate candidate)
        {
            _context.Candidates.Update(candidate);
            _context.SaveChanges();
        }
    }
}
