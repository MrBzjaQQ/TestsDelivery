using AutoMapper;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Models.Subject;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.Subjects
{
    public class SubjectsRepository : BaseRepository<TestsDeliveryContext, Subject>, ISubjectsRepository
    {
        public SubjectsRepository(TestsDeliveryContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
