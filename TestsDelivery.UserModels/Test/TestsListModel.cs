namespace TestsDelivery.UserModels.Test
{
    public record TestsListModel
    {
        public IEnumerable<TestInListModel> Tests { get; set; }

        public int TotalCount { get; set; }
    }
}
