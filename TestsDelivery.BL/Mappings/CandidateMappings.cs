using AutoMapper;
using TestsDelivery.BL.Models.Candidate;
using TestsDelivery.Domain.Candidate;

namespace TestsDelivery.BL.Mappings
{
    public class CandidateMappings : Profile
    {
        public CandidateMappings()
        {
            CreateMap<CandidateCreateModel, Candidate>();
            CreateMap<Candidate, CandidateReadModel>();
            CreateMap<CandidateEditModel, Candidate>();

            CreateMap<Candidate, DAL.Models.Candidate.Candidate>();
            CreateMap<DAL.Models.Candidate.Candidate, Candidate>();
        }
    }
}
