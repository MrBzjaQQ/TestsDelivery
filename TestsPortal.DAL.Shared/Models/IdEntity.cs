using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.DAL.Shared.Models
{
    public record IdEntity<TKey>
    {
        [Key]
        public TKey Id { get; set; }
    }
}
