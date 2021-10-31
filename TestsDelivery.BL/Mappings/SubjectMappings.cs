using AutoMapper;
using TestsDelivery.BL.Models.Subject;
using TestsDelivery.Domain.Subject;

namespace TestsDelivery.BL.Mappings
{
    public class SubjectMappings: Profile
    {
        public SubjectMappings()
        {
            CreateMap<Subject, SubjectReadModel>();
            CreateMap<Subject, SubjectInListModel>();
            CreateMap<SubjectCreateModel, Subject>();
            CreateMap<SubjectEditModel, Subject>();

            CreateMap<Subject, DAL.Models.Subject.Subject>();
            CreateMap<DAL.Models.Subject.Subject, Subject>();
        }
    }
}
