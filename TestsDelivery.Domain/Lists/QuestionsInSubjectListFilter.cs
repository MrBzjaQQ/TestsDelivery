namespace TestsDelivery.Domain.Lists
{
    public record QuestionsInSubjectListFilter : ListFilter
    {
        public long SubjectId { get; set; }
    }
}
