using TestsDelivery.BL.Models.Questions.Essay;

namespace TestsDelivery.BL.Validators.Questions
{
    public interface IEssayValidator
    {
        void ValidateCreateModel(EssayCreateModel model);

        void ValidateEditModel(EssayEditModel model);
    }
}
