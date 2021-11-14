using TestsDelivery.BL.Models.Questions.MultipleChoice;

namespace TestsDelivery.BL.Validators.Questions
{
    public interface IMcqModelValidator
    {
        void ValidateCreateModel(McqCreateModel model);

        void ValidateEditModel(McqEditModel model);
    }
}
