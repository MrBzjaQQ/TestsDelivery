using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.BL.Models.Subject
{
    public record SubjectEditModel : SubjectBaseModel
    {
        [Required]
        public bool Retired { get; set; }
    }
}
