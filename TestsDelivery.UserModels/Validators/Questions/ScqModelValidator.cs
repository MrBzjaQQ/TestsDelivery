using TestsDelivery.UserModels.Exceptions.Validation;
using TestsDelivery.UserModels.AnswerOptions;
using TestsDelivery.UserModels.Questions.SingleChoice;

namespace TestsDelivery.UserModels.Validators.Questions
{
    public class ScqModelValidator : BaseQuestionValidator<ScqCreateModel, ScqEditModel>, IScqModelValidator
    {
        public override void ValidateCreateModel(ScqCreateModel model)
        {
            if (model.AnswerOptions == null)
                throw new ArgumentNullException(nameof(model.AnswerOptions));

            base.ValidateCreateModel(model);

            ValidateAnswerOptions(model.AnswerOptions);
        }

        public override void ValidateEditModel(ScqEditModel model)
        {
            if (model.AnswerOptions == null)
                throw new ArgumentNullException(nameof(model.AnswerOptions));

            base.ValidateEditModel(model);

            ValidateAnswerOptions(model.AnswerOptions);
        }

        private void ValidateAnswerOptions(IEnumerable<AnswerOptionModelBase> options)
        {
            var correctOptions = options
                .Where(x => x.IsCorrect)
                .ToList();

            if (correctOptions.Count == 0)
                throw new NoCorrectAnswerOptionException();

            if (correctOptions.Count > 1)
                throw new MultipleCorrectAnswersException();
        }
    }
}
