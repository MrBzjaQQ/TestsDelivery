namespace TestsDelivery.UserModels.Subject
{
    public record SubjectsListModel
    {
        public IEnumerable<SubjectInListModel> Subjects { get; set; }

        public int TotalCount { get; set; }
    }
}
