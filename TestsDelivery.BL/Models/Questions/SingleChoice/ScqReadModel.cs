﻿using System.Collections.Generic;
using TestsDelivery.BL.Models.Questions.AnswerOptions;
using TestsDelivery.BL.Models.Questions.BaseQuestion;

namespace TestsDelivery.BL.Models.Questions.SingleChoice
{
    public record ScqReadModel : BaseQuestionReadModel
    {
        public IEnumerable<AnswerOptionReadModel> AnswerOptions { get; set; }
    }
}
