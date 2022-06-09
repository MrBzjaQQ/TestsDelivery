namespace TestsDelivery.UserModels.ScheduledTest
{
    // TODO: create model ResponseWithPagination<TModel>
    public record ScheduledTestsListModel
    {
        public IEnumerable<ScheduledTestInListModel> ScheduledTests { get; set; }

        public int TotalCount { get; set; }
    }
}
