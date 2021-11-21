using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.DAL.Models.Test
{
    public record Test
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
