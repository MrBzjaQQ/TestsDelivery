using TestsDelivery.BL.Models.Questions.Essay;

namespace TestsDelivery.BL.Mediators.Questions.Essay
{
    public interface IEssayMediator
    {
        EssayReadModel CreateQuestion(EssayCreateModel model);

        void EditQuestion(EssayEditModel model);

        EssayReadModel GetQuestion(long id);
    }
}
