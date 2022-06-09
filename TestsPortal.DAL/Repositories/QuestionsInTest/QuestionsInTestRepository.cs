using AutoMapper;
using TestsDelivery.DAL.Shared.Repository;
using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Tests;

namespace TestsPortal.DAL.Repositories.QuestionsInTest
{
    public class QuestionsInTestRepository : BaseRepository<TestsPortalContext, QuestionInTest>, IQuestionsInTestRepository
    {
        public QuestionsInTestRepository(TestsPortalContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
