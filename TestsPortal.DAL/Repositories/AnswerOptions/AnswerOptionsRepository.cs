using AutoMapper;
using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Repositories.AnswerOptions
{
    public class AnswerOptionsRepository : BaseRepository<AnswerOption>, IAnswerOptionsRepository
    {
        public AnswerOptionsRepository(TestsPortalContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public void CreateAnswerOptions(IEnumerable<AnswerOption> options)
        {
            Context.AnswerOptions.AddRange(options);
            Context.SaveChanges();
        }

        public IEnumerable<AnswerOption> GetAnswerOptionsByQuestionId(long questionId)
        {
            return Context.AnswerOptions.Where(x => x.QuestionId == questionId).ToList();
        }
    }
}
