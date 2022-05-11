using AutoMapper;
using System.Collections.Generic;
using TestsDelivery.BL.FilterBuilders.Subjects;
using TestsDelivery.DAL.Repositories.Subjects;
using TestsDelivery.Domain.Lists;
using TestsDelivery.Domain.Subject;

namespace TestsDelivery.BL.Services.Subjects
{
    public class SubjectsService : ISubjectsService
    {
        private readonly ISubjectsRepository _subjectsRepository;
        private readonly IMapper _mapper;

        public SubjectsService(ISubjectsRepository subjectsRepository, IMapper mapper)
        {
            _subjectsRepository = subjectsRepository;
            _mapper = mapper;
        }

        public Subject CreateSubject(Subject subject)
        {
            var subjectData = _mapper.Map<DAL.Models.Subject.Subject>(subject);

            _subjectsRepository.Create(subjectData);

            var createdSubject = _mapper.Map<Subject>(subjectData);

            return createdSubject;
        }

        public Subject GetSubject(long id)
        {
            return _mapper.Map<Subject>(_subjectsRepository.GetById(id));
        }

        public void EditSubject(Subject subject)
        {
            var subjectData = _mapper.Map<DAL.Models.Subject.Subject>(subject);

            _subjectsRepository.Update(subjectData);
        }

        public SubjectsList GetList(ListFilter filter)
        {
            var filterBuilder = new SubjectsFilterBuilder();

            if (filter.SearchText != null)
                filterBuilder.ByName(filter.SearchText);

            if (filter.Take.HasValue)
                filterBuilder.Take(filter.Take.Value);

            if (filter.Skip.HasValue)
                filterBuilder.Skip(filter.Skip.Value);

            filterBuilder.ByIsRetired(false);

            var genericFilter = filterBuilder.Build();

            return new SubjectsList
            {
                Subjects = _mapper.Map<IList<SubjectInListDto>>(_subjectsRepository.GetWithProjection<DAL.Models.Subject.SubjectInList>(genericFilter)),
                TotalCount = _subjectsRepository.Count(genericFilter),
            };
        }
    }
}
