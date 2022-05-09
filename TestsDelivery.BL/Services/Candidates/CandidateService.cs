using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TestsDelivery.BL.FilterBuilders.Candidates;
using TestsDelivery.DAL.Exceptions.Candidate;
using TestsDelivery.DAL.Repositories.Candidate;
using TestsDelivery.Domain.Candidate;
using TestsDelivery.Domain.Lists;

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
            _repository.Create(candidateData);
            var createdCandidate = _mapper.Map<Candidate>(candidateData);

            return createdCandidate;
        }

        public Candidate GetCandidate(long id)
        {
            try
            {
                return _mapper.Map<Candidate>(_repository.GetById(id));
            }
            catch (InvalidOperationException)
            {
                throw new CandidateNotFoundException(id);
            }
        }

        public IEnumerable<Candidate> GetCandidates(IEnumerable<long> candidateIds)
        {
            var candidates = _repository.GetCandidates(candidateIds);

            if (candidates.Count() != candidateIds.Count())
            {
                var missingIds = candidateIds.Except(candidates.Select(x => x.Id));
                throw new CandidatesNotFountException(missingIds);
            }

            return _mapper.Map<IEnumerable<Candidate>>(candidates);
        }

        public void EditCandidate(Candidate candidate)
        {
            var candidateData = _mapper.Map<DAL.Models.Candidate.Candidate>(candidate);
            _repository.Update(candidateData);
        }

        public CandidatesList GetList(ListFilter filter)
        {
            var filterBuilder = new CandidatesFilterBuilder();

            if (filter.SearchText != null)
                filterBuilder.ByName(filter.SearchText);

            if (filter.Take.HasValue)
                filterBuilder.Take(filter.Take.Value);

            if (filter.Skip.HasValue)
                filterBuilder.Skip(filter.Skip.Value);

            var genericFilter = filterBuilder.Build();

            return new CandidatesList
            {
                Candidates = _mapper.Map<IEnumerable<Candidate>>(_repository.GetByFilter(genericFilter)),
                TotalCount = _repository.Count()
            };
        }
    }
}
