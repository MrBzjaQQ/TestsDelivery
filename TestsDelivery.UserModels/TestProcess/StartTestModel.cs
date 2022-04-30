namespace TestsDelivery.UserModels.TestProcess
{
    public record StartTestModel
    {
        public long TestId { get; set; }

        public string Keycode { get; set; }

        public string Pin { get; set; }
    }
}
