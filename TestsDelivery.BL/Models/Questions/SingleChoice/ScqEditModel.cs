﻿using System.Collections.Generic;
using TestsDelivery.BL.Models.Questions.AnswerOptions;
using TestsDelivery.BL.Models.Questions.BaseQuestion;

namespace TestsDelivery.BL.Models.Questions.SingleChoice
{
    public record ScqEditModel : BaseQuestionEditModel
    {
        public IEnumerable<AnswerOptionEditModel> AnswerOptions { get; set; }
    }
}
