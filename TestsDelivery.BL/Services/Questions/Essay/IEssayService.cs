using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Services.Questions.Essay
{
    public interface IEssayService
    {
        EssayQuestion CreateQuestion(EssayQuestion question);

        void EditQuestion(EssayQuestion question);

        EssayQuestion GetQuestion(long id);
    }
}
