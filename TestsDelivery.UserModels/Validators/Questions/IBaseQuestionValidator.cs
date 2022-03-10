using TestsDelivery.UserModels.Questions.BaseQuestion;

namespace TestsDelivery.UserModels.Validators.Questions
{
    public interface IBaseQuestionValidator<in TCreateModel, in TEditModel>
    {
        void ValidateCreateModel(TCreateModel model);

        void ValidateEditModel(TEditModel model);
    }
}
