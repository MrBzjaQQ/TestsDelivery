using System;
using System.Linq;
using AutoMapper;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Exceptions.Subjects;
using TestsDelivery.DAL.Models.Subject;

namespace TestsDelivery.DAL.Repositories.Subjects
{
    public class SubjectsRepository : ISubjectsRepository
    {
        private readonly TestsDeliveryContext _context;

        public SubjectsRepository(TestsDeliveryContext context)
        {
            _context = context;
        }

        public void CreateSubject(Subject subject)
        {
            _context.Subjects.Add(subject);
            _context.SaveChanges();
        }

        public Subject GetSubject(int id)
        {
            try
            {
                return _context.Subjects.Single(subject => subject.Id == id);
            }
            catch (InvalidOperationException)
            {
                throw new SubjectNotFoundException(id);
            }
        }

        public void EditSubject(Subject subject)
        {
            _context.Subjects.Update(subject);
            _context.SaveChanges();
        }
    }
}
