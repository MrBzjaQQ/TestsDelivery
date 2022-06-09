using TestsDelivery.DAL.Shared.Models;

namespace TestsPortal.DAL.Models
{
    public record IdOriginalIdEntity<TKey> : IdEntity<TKey>
    {
        public TKey OriginalId { get; set; }
    }
}
