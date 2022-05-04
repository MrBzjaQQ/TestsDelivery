using System.Collections.Generic;
using TestsDelivery.BL.FilterBuilders.Questions;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.Domain.Lists;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Services.Questions
{
    public class QuestionListsService : IQuestionListsService
    {
        private readonly IQuestionsRepository _questionsRepository;

        public QuestionListsService(IQuestionsRepository questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }

        public IEnumerable<QuestionInListDto> GetList(ListFilter filter)
        {
            var filterBuilder = new QuestionsFilterBuilder();

            if (filter.SearchText != null)
                filterBuilder.ByName(filter.SearchText);

            if (filter.Take.HasValue)
                filterBuilder.Take(filter.Take.Value);

            if (filter.Skip.HasValue)
                filterBuilder.Skip(filter.Skip.Value);

            var genericFilter = filterBuilder.Build();

            return _questionsRepository.GetQuestionsInSubjects<QuestionInListDto>(genericFilter);
        }
    }
}
