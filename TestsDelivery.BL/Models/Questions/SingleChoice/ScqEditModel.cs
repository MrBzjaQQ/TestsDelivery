﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestsDelivery.BL.Models.Questions.AnswerOptions;

namespace TestsDelivery.BL.Models.Questions.SingleChoice
{
    public record ScqEditModel
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        public string Text { get; set; }

        public IEnumerable<AnswerOptionEditModel> AnswerOptions { get; set; }
    }
}
