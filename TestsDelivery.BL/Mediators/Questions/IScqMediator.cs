using TestsDelivery.BL.Models.Questions.SingleChoice;

namespace TestsDelivery.BL.Mediators.Questions
{
    public interface IScqMediator
    {
        ScqReadModel CreateQuestion(ScqCreateModel model);

        void EditQuestion(ScqEditModel model);

        ScqReadModel GetQuestion(long id);
    }
}
