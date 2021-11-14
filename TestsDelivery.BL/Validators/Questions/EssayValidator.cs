using System;
using TestsDelivery.BL.Models.Questions.Essay;

namespace TestsDelivery.BL.Validators.Questions
{
    public class EssayValidator : IEssayValidator
    {
        public void ValidateCreateModel(EssayCreateModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                throw new ArgumentNullException(nameof(model.Name));

            if (string.IsNullOrWhiteSpace(model.Question))
                throw new ArgumentNullException(nameof(model.Question));
        }

        public void ValidateEditModel(EssayEditModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                throw new ArgumentNullException(nameof(model.Name));

            if (string.IsNullOrWhiteSpace(model.Question))
                throw new ArgumentNullException(nameof(model.Question));
        }
    }
}
