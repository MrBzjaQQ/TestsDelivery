﻿using AutoMapper;
using TestsDelivery.DAL.Repositories.Subjects;
using TestsDelivery.Domain.Subject;

namespace TestsDelivery.BL.Services.Subjects
{
    public class SubjectsService: ISubjectsService
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

            _subjectsRepository.CreateSubject(subjectData);

            var createdSubject = _mapper.Map<Subject>(subjectData);

            return createdSubject;
        }

        public Subject GetSubject(long id)
        {
            return _mapper.Map<Subject>(_subjectsRepository.GetSubject(id));
        }

        public void EditSubject(Subject subject)
        {
            var subjectData = _mapper.Map<DAL.Models.Subject.Subject>(subject);

            _subjectsRepository.EditSubject(subjectData);
        }
    }
}
