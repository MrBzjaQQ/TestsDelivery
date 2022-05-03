using AutoMapper;
using TestsDelivery.UserModels.Candidate;
using TestsDelivery.BL.Services.Candidates;
using System.Collections.Generic;
using TestsDelivery.UserModels.Lists;
using TestsDelivery.Domain.Lists;

namespace TestsDelivery.BL.Mediators.Candidate
{
    public class CandidateMediator : ICandidateMediator
    {
        private readonly IMapper _mapper;
        private readonly ICandidateService _service;

        public CandidateMediator(ICandidateService service, IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }

        public CandidateReadModel CreateCandidate(CandidateCreateModel model)
        {
            var candidate = _mapper.Map<Domain.Candidate.Candidate>(model);

            return _mapper.Map<CandidateReadModel>(_service.CreateCandidate(candidate));
        }

        public CandidateReadModel GetCandidate(long id)
        {
            return _mapper.Map<CandidateReadModel>(_service.GetCandidate(id));
        }

        public void EditCandidate(CandidateEditModel model)
        {
            var candidate = _mapper.Map<Domain.Candidate.Candidate>(model);

            _service.EditCandidate(candidate);
        }

        public IEnumerable<CandidateReadModel> GetList(ListModel model)
        {
            var filter = _mapper.Map<ListFilter>(model);
            return _mapper.Map<IEnumerable<CandidateReadModel>>(_service.GetList(filter));
        }
    }
}
