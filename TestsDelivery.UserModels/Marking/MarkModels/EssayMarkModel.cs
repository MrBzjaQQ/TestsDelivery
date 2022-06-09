using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.UserModels.Marking.MarkModels
{
    public record EssayMarkModel
    {
        // TODO: Maximum mark should be flexible and stored in constants or configs
        [Range(0, 5)]
        public float Content { get; set; }

        [Range(0, 5)]
        public float Grammar { get; set; }

        [Range(0, 5)]
        public float Vocabulary { get; set; }

        [Range(0, 5)]
        public float Mechanics { get; set; }
    }
}
