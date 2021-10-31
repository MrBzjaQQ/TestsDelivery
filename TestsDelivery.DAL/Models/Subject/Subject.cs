using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.DAL.Models.Subject
{
    public record Subject
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Retired { get; set; }
    }
}
