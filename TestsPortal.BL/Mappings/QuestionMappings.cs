using AutoMapper;
using TestsDelivery.UserModels.Questions.BaseQuestion;
using TestsDelivery.UserModels.Questions.Essay;
using TestsDelivery.UserModels.Questions.MultipleChoice;
using TestsDelivery.UserModels.Questions.SingleChoice;
using TestsPortal.Domain.Questions;

namespace TestsPortal.BL.Mappings
{
    public class QuestionMappings : Profile
    {
        public QuestionMappings()
        {
            CreateMap<QuestionReadModel, QuestionBase>()
                .Include<QuestionWithOptionsReadModel, QuestionWithOptionsBase>()
                .Include<EssayReadModel, QuestionBase>()
                .Include<ScqReadModel, QuestionBase>()
                .Include<McqReadModel, QuestionBase>();
        }
    }
}
