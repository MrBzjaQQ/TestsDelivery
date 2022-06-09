using TestsDelivery.DAL.Shared.Models;
using TestsPortal.DAL.Models.Subject;

namespace TestsPortal.DAL.Models.Questions
{
    public record ShortQuestion : IdEntity<long>
    {
        public string Name { get; set; } = string.Empty;

        public ShortSubject? Subject { get; set; }
    }
}
