namespace TestsDelivery.Domain.Marking.Marks
{
    public record EssayMark
    {
        public float Content { get; set; }

        public float Grammar { get; set; }

        public float Vocabulary { get; set; }

        public float Mechanics { get; set; }
    }
}
