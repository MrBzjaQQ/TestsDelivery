using AutoMapper;
using TestsPortal.DAL.Repositories.Candidate;
using TestsPortal.Domain.Candidates;

namespace TestsPortal.BL.Services.Candidates
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidatesRepository _candidatesRepository;
        private readonly IMapper _mapper;

        public CandidateService(ICandidatesRepository candidatesRepository, IMapper mapper)
        {
            _candidatesRepository = candidatesRepository;
            _mapper = mapper;
        }

        public Candidate CreateCandidate(Candidate candidate)
        {
            var dalCandidate = _mapper.Map<DAL.Models.Candidates.Candidate>(candidate);
            _candidatesRepository.CreateCandidate(dalCandidate);
            return _mapper.Map<Candidate>(dalCandidate);
        }

        public IEnumerable<Candidate> CreateCandidates(IEnumerable<Candidate> candidates)
        {
            var dalCandidates = _mapper.Map<IEnumerable<DAL.Models.Candidates.Candidate>>(candidates);
            _candidatesRepository.CreateCandidates(dalCandidates);
            return _mapper.Map<IEnumerable<Candidate>>(dalCandidates);
        }
    }
}
