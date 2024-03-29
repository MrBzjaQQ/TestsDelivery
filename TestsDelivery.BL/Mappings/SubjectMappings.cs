﻿using AutoMapper;
using TestsDelivery.UserModels.Subject;
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

            CreateMap<SubjectInListModel, SubjectInListDto>()
                .ReverseMap();
            CreateMap<SubjectInListDto, DAL.Models.Subject.SubjectInList>()
                .ReverseMap();

            CreateMap<SubjectInListDto, SubjectWithQuestionsModel>();
            CreateMap<SubjectsList, SubjectsListModel>();
            CreateMap<DAL.Models.Subject.Subject, DAL.Models.Subject.SubjectInList>();
            CreateMap<DAL.Models.Subject.SubjectInList, SubjectInListDto>();
            CreateMap<DAL.Models.Subject.Subject, SubjectInListDto>();
        }
    }
}
