using TestsDelivery.DAL.Shared.Models;

namespace TestsDelivery.DAL.Models.Subject
{
    public record SubjectInList : IdEntity<long>
    {
        public string Name { get; set; }
    }
}
