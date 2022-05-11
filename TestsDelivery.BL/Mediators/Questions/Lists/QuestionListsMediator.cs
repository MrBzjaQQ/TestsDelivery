using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TestsDelivery.BL.Services.Questions;
using TestsDelivery.Domain.Lists;
using TestsDelivery.Domain.Questions;
using TestsDelivery.UserModels.ListFilters;
using TestsDelivery.UserModels.Questions;
using TestsDelivery.UserModels.Subject;

namespace TestsDelivery.BL.Mediators.Questions.Lists
{
    public class QuestionListsMediator : IQuestionListsMediator
    {
        private readonly IQuestionListsService _service;
        private readonly IMapper _mapper;

        public QuestionListsMediator(IQuestionListsService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public QuestionsListModel GetQuestionsForSubject(QuestionsInSubjectListFilterModel model)
        {
            var filter = _mapper.Map<QuestionsInSubjectListFilter>(model);
            return _mapper.Map<QuestionsListModel>(_service.GetQuestionsForSubject(filter));
        }

        public IEnumerable<SubjectWithQuestionsModel> GetQuestionsInSubjects(ListFilterModel model)
        {
            var list = _service.GetList(_mapper.Map<ListFilter>(model));
            return MapQuestionsToSubjectsWithQuestions(list);
        }

        private IEnumerable<SubjectWithQuestionsModel> MapQuestionsToSubjectsWithQuestions(IEnumerable<QuestionInListDto> list)
        {
            var groups = list.GroupBy(x => x.Subject);
            var targetList = new List<SubjectWithQuestionsModel>();

            foreach(var group in groups)
            {
                var subject = _mapper.Map<SubjectWithQuestionsModel>(group.Key);
                subject.Questions = _mapper.Map<IEnumerable<QuestionInSubjectModel>>(group.AsEnumerable());
                yield return subject;
            }
        }
    }
}
