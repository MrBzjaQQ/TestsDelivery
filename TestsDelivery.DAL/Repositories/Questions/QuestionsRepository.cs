using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Exceptions.Questions;
using TestsDelivery.DAL.Models.Questions;

namespace TestsDelivery.DAL.Repositories.Questions
{
    public class QuestionsRepository : IQuestionsRepository
    {
        private readonly TestsDeliveryContext _context;

        public QuestionsRepository(TestsDeliveryContext context)
        {
            _context = context;
        }

        public void CreateQuestion(Question question)
        {
            _context.Questions.Add(question);
            _context.SaveChanges();
        }

        public void EditQuestion(Question question)
        {
            try
            {
                var existingQuestion = _context.Questions
                    .Single(x => x.Id == question.Id);

                if (existingQuestion.Type != question.Type)
                    throw new QuestionIncorrectTypeException(question.Type, existingQuestion.Type);

                ApplyChangesToDestination(question, existingQuestion);
                _context.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                throw new QuestionNotFoundException(question.Id);
            }
        }

        public Question GetQuestion(long id, short questionType)
        {
            var question = GetQuestion(id);
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

        public Question GetQuestion(long id)
        {
            try
            {
                return _context.Questions
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
            return _context.QuestionInTests
                .Where(questionInTest => questionInTest.TestId == testId)
                .Join(
                    _context.Questions,
                    questionInTest => questionInTest.QuestionId,
                    question => question.Id,
                    (questionInTest, question) => question)
                .Include(x => x.Subject);
        }
    }
}
