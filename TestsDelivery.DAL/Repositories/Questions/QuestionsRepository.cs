using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Exceptions.Questions;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Shared;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.Questions
{
    public class QuestionsRepository : BaseRepository<TestsDeliveryContext, Question>, IQuestionsRepository
    {
        public QuestionsRepository(TestsDeliveryContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public override void Update(Question question)
        {
            try
            {
                var existingQuestion = Context.Questions
                    .Single(x => x.Id == question.Id);

                if (existingQuestion.Type != question.Type)
                    throw new QuestionIncorrectTypeException(question.Type, existingQuestion.Type);

                ApplyChangesToDestination(question, existingQuestion);
                Context.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                throw new QuestionNotFoundException(question.Id);
            }
        }

        public Question GetQuestion(long id, short questionType)
        {
            var question = GetById(id);
            if (question.Type != questionType)
                throw new QuestionIncorrectTypeException(questionType, question.Type);

            return question;
        }

        public IEnumerable<Question> GetQuestionsByTestId(long testId)
        {
            return GetQuestionsByTestIdInternal(testId)
                    .ToList();
        }

        public IEnumerable<long> GetQuestionIdsByTestId(long testId)
        {
            return GetQuestionsByTestIdInternal(testId)
                .Select(x => x.Id)
                .ToList();
        }

        public override Question GetById(long id)
        {
            try
            {
                return Context.Questions
                    .Include(x => x.Subject)
                    .Single(x => x.Id == id);
            }
            catch (InvalidOperationException)
            {
                throw new QuestionNotFoundException(id);
            }
        }

        private void ApplyChangesToDestination(Question source, Question destination)
        {
            destination.Name = source.Name;
            destination.Text = source.Text;
        }

        private IQueryable<Question> GetQuestionsByTestIdInternal(long testId)
        {
            return Context.QuestionInTests
                .Where(questionInTest => questionInTest.TestId == testId)
                .Join(
                    Context.Questions,
                    questionInTest => questionInTest.QuestionId,
                    question => question.Id,
                    (questionInTest, question) => question)
                .Include(x => x.Subject);
        }

        public void DeleteQuestion(long id, short questionType)
        {
            var question = Context.Questions.Find(id);
            if (question != null)
            {
                if (question.Type != questionType)
                    throw new QuestionIncorrectTypeException(questionType, question.Type);

                Context.Questions.Remove(question);
            }
        }

        public IEnumerable<TDomain> GetQuestionsInSubjects<TDomain>(GenericFilter<Question> filter)
        {
            var query = ApplyFilter(Context.Questions.Include(x => x.Subject), filter);
            return Mapper.ProjectTo<TDomain>(query);
        }
    }
}
