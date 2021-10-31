using System;
using System.Linq;
using AutoMapper;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Models.Subject;

namespace TestsDelivery.DAL.Repositories.Subjects
{
    public class SubjectsRepository: ISubjectsRepository
    {
        private readonly TestsDeliveryContext _context;
        private readonly IMapper _mapper;

        public SubjectsRepository(TestsDeliveryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void CreateSubject(Subject subject)
        {
            _context.Subjects.Add(subject);
            _context.SaveChanges();
        }

        public Subject GetSubject(int id)
        {
            throw new NotImplementedException();
        }

        public void EditSubject(Subject subject)
        {
            throw new NotImplementedException();
        }
    }
}
