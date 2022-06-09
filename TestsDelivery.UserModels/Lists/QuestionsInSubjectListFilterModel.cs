using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.UserModels.ListFilters
{
    public record QuestionsInSubjectListFilterModel : ListFilterModel
    {
        [Range(1, long.MaxValue)]
        public long SubjectId { get; set; }
    }
}
