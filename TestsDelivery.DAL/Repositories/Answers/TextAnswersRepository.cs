using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.Answers
{
    public class TextAnswersRepository : BaseRepository<TestsDeliveryContext, TextAnswer>, ITextAnswersRepository
    {
        public TextAnswersRepository(TestsDeliveryContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public IEnumerable<TextAnswer> GetByTestId(long testId)
        {
            return Context.TextAnswers
                .Where(x => x.ScheduledTestId == testId)
                .ToList();
        }
    }
}
