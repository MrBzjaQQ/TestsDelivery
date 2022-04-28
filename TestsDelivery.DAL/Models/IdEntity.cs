using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.DAL.Models
{
    public record IdEntity<TKey>
    {
        [Key]
        public TKey Id { get; set; }
    }
}
