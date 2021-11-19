﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestsDelivery.BL.Models.Questions.AnswerOptions;
using TestsDelivery.BL.Models.Questions.BaseQuestion;

namespace TestsDelivery.BL.Models.Questions.SingleChoice
{
    public record ScqCreateModel : BaseQuestionCreateModel
    {
        [MinLength(2)]
        public IEnumerable<AnswerOptionCreateModel> AnswerOptions { get; set; }
    }
}
