using TestsDelivery.BL.Models.Questions.SingleChoice;

namespace TestsDelivery.BL.Validators.Questions
{
    public interface IScqModelValidator
    {
        void ValidateCreateModel(ScqCreateModel model);

        void ValidateEditModel(ScqEditModel model);
    }
}
