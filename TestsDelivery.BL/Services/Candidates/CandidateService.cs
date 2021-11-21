using AutoMapper;
using TestsDelivery.DAL.Repositories.Candidate;
using TestsDelivery.Domain.Candidate;

namespace TestsDelivery.BL.Services.Candidates
{
    public class CandidateService : ICandidateService
    {
        private readonly IMapper _mapper;
        private readonly ICandidateRepository _repository;

        public CandidateService(ICandidateRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public Candidate CreateCandidate(Candidate candidate)
        {
            var candidateData = _mapper.Map<DAL.Models.Candidate.Candidate>(candidate);

            _repository.CreateCandidate(candidateData);

            var createdCandidate = _mapper.Map<Candidate>(candidateData);

            return createdCandidate;
        }

        public Candidate GetCandidate(long id)
        {
            return _mapper.Map<Candidate>(_repository.GetCandidate(id));
        }

        public void EditCandidate(Candidate candidate)
        {
            var candidateData = _mapper.Map<DAL.Models.Candidate.Candidate>(candidate);

            _repository.EditCandidate(candidateData);
        }
    }
}
