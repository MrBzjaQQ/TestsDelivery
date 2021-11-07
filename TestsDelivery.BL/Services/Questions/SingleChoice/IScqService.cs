using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Services.Questions.SingleChoice
{
    public interface IScqService
    {
        SingleChoiceQuestion CreateQuestion(SingleChoiceQuestion question);

        void EditQuestion(SingleChoiceQuestion question);

        SingleChoiceQuestion GetQuestion(long id);
    }
}
