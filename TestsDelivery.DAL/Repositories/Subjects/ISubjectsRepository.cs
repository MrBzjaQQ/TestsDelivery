﻿using TestsDelivery.DAL.Models.Subject;

namespace TestsDelivery.DAL.Repositories.Subjects
{
    public interface ISubjectsRepository
    {
        void CreateSubject(Subject subject);

        Subject GetSubject(int id);

        void EditSubject(Subject subject);
    }
}