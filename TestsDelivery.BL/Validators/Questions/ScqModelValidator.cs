using System;
using System.Collections.Generic;
using System.Linq;
using TestsDelivery.BL.Exceptions.Validation;
using TestsDelivery.BL.Models.Questions.AnswerOptions;
using TestsDelivery.BL.Models.Questions.SingleChoice;

namespace TestsDelivery.BL.Validators.Questions
{
    public class ScqModelValidator : IScqModelValidator
    {
        public void ValidateCreateModel(ScqCreateModel model)
        {
            if (model.AnswerOptions == null)
                throw new ArgumentNullException(nameof(model.AnswerOptions));

            ValidateAnswerOptions(model.AnswerOptions);
        }

        public void ValidateEditModel(ScqEditModel model)
        {
            if (model.AnswerOptions == null)
                throw new ArgumentNullException(nameof(model.AnswerOptions));

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
