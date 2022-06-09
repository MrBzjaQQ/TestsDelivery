namespace TestsDelivery.Domain.Lists
{
    public record ListFilter
    {
        public int? Take { get; set; }

        public int? Skip { get; set; }

        public string SearchText { get; set; }
    }
}
