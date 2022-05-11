namespace TestsDelivery.UserModels.ListFilters
{
    public record ListFilterModel
    {
        public int? Take { get; set; }

        public int? Skip { get; set; }

        public string? SearchText { get; set; }
    }
}
