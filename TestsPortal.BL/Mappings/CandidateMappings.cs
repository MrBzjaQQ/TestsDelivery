using AutoMapper;
using TestsDelivery.UserModels.Candidate;
using TestsPortal.Domain.Candidates;

namespace TestsPortal.BL.Mappings
{
    public class CandidateMappings : Profile
    {
        public CandidateMappings()
        {
            CreateMap<CandidateReadModel, Candidate>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id));
            CreateMap<Candidate, DAL.Models.Candidates.Candidate>();
            CreateMap<DAL.Models.Candidates.Candidate, Candidate>();
            CreateMap<Candidate, CandidateReadModel>();
        }
    }
}
