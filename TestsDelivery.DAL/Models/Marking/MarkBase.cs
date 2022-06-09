using System.ComponentModel.DataAnnotations.Schema;
using TestsDelivery.DAL.Shared.Models;

namespace TestsDelivery.DAL.Models.Marking
{
    public abstract record MarkBase : IdEntity<long>
    {
        [ForeignKey(nameof(Questions.Question))]
        public long QuestionId { get; set; }

        [ForeignKey(nameof(ScheduledTest.ScheduledTestInstance))]
        public long TestInstanceId { get; set; }

        public string Comment { get; set; }
    }
}
