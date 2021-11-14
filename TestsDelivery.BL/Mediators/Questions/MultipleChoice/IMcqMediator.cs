using TestsDelivery.BL.Models.Questions.MultipleChoice;

namespace TestsDelivery.BL.Mediators.Questions.MultipleChoice
{
    public interface IMcqMediator
    {
        McqReadModel CreateQuestion(McqCreateModel model);

        void EditQuestion(McqEditModel model);

        McqReadModel GetQuestion(long id);
    }
}
