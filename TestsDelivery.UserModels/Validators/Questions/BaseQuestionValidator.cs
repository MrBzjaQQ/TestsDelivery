using TestsDelivery.UserModels.Questions.BaseQuestion;

namespace TestsDelivery.UserModels.Validators.Questions
{
    public class BaseQuestionValidator<TCreateModel, TEditModel> : IBaseQuestionValidator<TCreateModel, TEditModel>
        where TCreateModel: QuestionCreateModel
        where TEditModel: QuestionEditModel
    {
        public virtual void ValidateCreateModel(TCreateModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                throw new ArgumentException(nameof(model.Name));

            if (string.IsNullOrWhiteSpace(model.Text))
                throw new ArgumentException(nameof(model.Text));
        }

        public virtual void ValidateEditModel(TEditModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                throw new ArgumentException(nameof(model.Name));

            if (string.IsNullOrWhiteSpace(model.Text))
                throw new ArgumentException(nameof(model.Text));
        }
    }
}
