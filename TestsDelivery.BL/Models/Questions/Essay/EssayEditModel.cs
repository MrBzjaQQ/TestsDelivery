﻿using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.BL.Models.Questions.Essay
{
    public record EssayEditModel
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Question { get; set; }

        [Range(1, long.MaxValue)]
        public long SubjectId { get; set; }
    }
}