using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.UserModels.Subject
{
    // TODO: something wrong with these models.
    // It needs to get rid of unnecessary ones.
    public record SubjectBaseModel
    {
        [Range(1, int.MaxValue)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
