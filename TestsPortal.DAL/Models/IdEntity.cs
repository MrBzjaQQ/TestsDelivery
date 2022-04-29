using System.ComponentModel.DataAnnotations;

namespace TestsPortal.DAL.Models
{
    public record IdEntity<TKey>
    {
        [Key]
        public TKey Id { get; set; }

        public TKey OriginalId { get; set; }
    }
}
