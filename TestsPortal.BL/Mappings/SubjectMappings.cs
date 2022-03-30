using AutoMapper;
using DomainSubject = TestsPortal.Domain.Subjects.Subject;
using DALSubject = TestsPortal.DAL.Models.Subject.Subject;
using TestsDelivery.UserModels.Subject;

namespace TestsPortal.BL.Mappings
{
    public class SubjectMappings : Profile
    {
        public SubjectMappings()
        {
            CreateMap<SubjectReadModel, DomainSubject>()
                .ReverseMap();

            CreateMap<DomainSubject, DALSubject>()
                .ReverseMap();
        }
    }
}
