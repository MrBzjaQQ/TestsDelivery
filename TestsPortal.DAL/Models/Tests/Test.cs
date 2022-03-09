using System.ComponentModel.DataAnnotations;

namespace TestsPortal.DAL.Models.Tests
{
    public record Test
    {
        [Key]
        public long Id { get; set; }

        public long OriginalId { get; set; }

        public string Name { get; set; }
    }
}
