using System;
using System.Linq;
using AutoMapper;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Exceptions.Subjects;
using TestsDelivery.DAL.Models.Subject;

namespace TestsDelivery.DAL.Repositories.Subjects
{
    public class SubjectsRepository : BaseRepository<Subject>, ISubjectsRepository
    {
        public SubjectsRepository(TestsDeliveryContext context)
            : base(context)
        {
        }
    }
}
