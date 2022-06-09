using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.Answers
{
    public class ChoiceAnswersRepository : BaseRepository<TestsDeliveryContext, ChoiceAnswer>, IChoiceAnswersRepository
    {
        public ChoiceAnswersRepository(TestsDeliveryContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public IEnumerable<ChoiceAnswer> GetByTestId(long testId)
        {
            return Context.ChoiceAnswers
                .Where(x => x.ScheduledTestId == testId)
                .ToList();
        }
    }
}
