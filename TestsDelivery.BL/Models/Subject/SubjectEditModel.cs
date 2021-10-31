using System;
using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.BL.Models.Subject
{
    public record SubjectEditModel
    {
        [Range(1, Int32.MaxValue)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool Retired { get; set; }
    }
}
