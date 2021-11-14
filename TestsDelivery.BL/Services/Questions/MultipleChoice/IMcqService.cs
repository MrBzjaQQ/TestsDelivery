using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Services.Questions.MultipleChoice
{
    public interface IMcqService
    {
        MultipleChoiceQuestion CreateQuestion(MultipleChoiceQuestion question);

        void EditQuestion(MultipleChoiceQuestion question);

        MultipleChoiceQuestion GetQuestion(long id);
    }
}
