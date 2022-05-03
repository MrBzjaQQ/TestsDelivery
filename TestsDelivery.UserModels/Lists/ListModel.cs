namespace TestsDelivery.UserModels.Lists
{
    public record ListModel
    {
        public int? Take { get; set; }

        public int? Skip { get; set; }

        public string? SearchText { get; set; }
    }
}
