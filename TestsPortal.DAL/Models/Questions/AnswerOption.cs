﻿using System.ComponentModel.DataAnnotations.Schema;

namespace TestsPortal.DAL.Models.Questions
{
    public record AnswerOption : IdOriginalIdEntity<long>
    {
        [ForeignKey(nameof(Question))]
        public long QuestionId { get; set; }

        public string Text { get; set; } = string.Empty;
    }
}
