namespace TestsDelivery.DAL.Models.Marking
{
    public record EssayMark : MarkBase
    { 
        public float Content { get; set; }

        public float Grammar { get; set; }

        public float Vocabulary { get; set; }

        public float Mechanics { get; set; }
    }
}
