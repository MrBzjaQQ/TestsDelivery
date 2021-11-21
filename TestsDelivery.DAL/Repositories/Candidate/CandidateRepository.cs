using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Exceptions.Candidate;

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
            try
            {
                return _context.Candidates.Single(c => c.Id == id);
            }
            catch (InvalidOperationException)
            {
                throw new CandidateNotFoundException(id);
            }
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
