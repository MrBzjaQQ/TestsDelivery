﻿using AutoMapper;
using TestsDelivery.UserModels.Subject;
using TestsDelivery.BL.Services.Subjects;
using TestsDelivery.Domain.Subject;
using System.Collections.Generic;
using TestsDelivery.UserModels.Lists;
using TestsDelivery.Domain.Lists;

namespace TestsDelivery.BL.Mediators.Subjects
{
    public class SubjectsMediator: ISubjectsMediator
    {
        private readonly ISubjectsService _subjectsService;
        private readonly IMapper _mapper;

        public SubjectsMediator(ISubjectsService subjectsService, IMapper mapper)
        {
            _subjectsService = subjectsService;
            _mapper = mapper;
        }

        public SubjectReadModel CreateSubject(SubjectCreateModel model)
        {
            var subject = _mapper.Map<Subject>(model);

            return _mapper.Map<SubjectReadModel>(_subjectsService.CreateSubject(subject));
        }

        public SubjectReadModel GetSubject(long id)
        {
            return _mapper.Map<SubjectReadModel>(_subjectsService.GetSubject(id));
        }

        public void EditSubject(SubjectEditModel model)
        {
            var subject = _mapper.Map<Subject>(model);

            _subjectsService.EditSubject(subject);
        }

        public IEnumerable<SubjectInListModel> GetList(ListModel listModel)
        {
            var subjectsInList = _subjectsService.GetList(_mapper.Map<ListFilter>(listModel));
            return _mapper.Map<IEnumerable<SubjectInListModel>>(subjectsInList);
        }
    }
}
