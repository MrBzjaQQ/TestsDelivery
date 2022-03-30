using AutoMapper;
using TestsPortal.Domain.Subjects;
using TestsPortal.DAL.Repositories.Subjects;

namespace TestsPortal.BL.Services.Subjects
{
    public class SubjectsService : ISubjectsService
    {
        private readonly ISubjectsRepository _subjectsRepository;
        private readonly IMapper _mapper;

        public SubjectsService(
            ISubjectsRepository subjectsRepository,
            IMapper mapper)
        {
            _subjectsRepository = subjectsRepository;
            _mapper = mapper;
        }

        public Subject CreateSubject(Subject subject)
        {
            var dalSubject = _mapper.Map<DAL.Models.Subject.Subject>(subject);
            _subjectsRepository.CreateSubject(dalSubject);
            return _mapper.Map<Subject>(dalSubject);
        }
    }
}
