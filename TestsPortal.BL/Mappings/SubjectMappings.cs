using AutoMapper;
using DomainSubject = TestsPortal.Domain.Subjects.Subject;
using DALSubject = TestsPortal.DAL.Models.Subject.Subject;
using TestsDelivery.UserModels.Subject;
using DALShortSubject = TestsPortal.DAL.Models.Subject.ShortSubject;
using DomainShortSubject = TestsPortal.Domain.Subjects.ShortSubject;
using TestsDelivery.UserModels.Questions.BaseQuestion;

namespace TestsPortal.BL.Mappings
{
    public class SubjectMappings : Profile
    {
        public SubjectMappings()
        {
            CreateMap<SubjectReadModel, DomainSubject>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(x => x.Id))
                .ReverseMap();

            CreateMap<DomainSubject, DALSubject>()
                .ReverseMap();

            CreateMap<DALSubject, DALShortSubject>();
            CreateMap<DALShortSubject, DomainShortSubject>();
            CreateMap<DomainShortSubject, SubjectInListModel>();
            CreateMap<DomainShortSubject, SubjectInList>();
        }
    }
}
