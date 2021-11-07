using System;
using System.Collections.Generic;
using System.Linq;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Exceptions.AnswerOptions;
using TestsDelivery.DAL.Models.Questions;

namespace TestsDelivery.DAL.Repositories.AnswerOptions
{
    public class AnswerOptionsRepository : IAnswerOptionsRepository
    {
        private readonly TestsDeliveryContext _context;

        public AnswerOptionsRepository(TestsDeliveryContext context)
        {
            _context = context;
        }

        public void CreateAnswerOption(AnswerOption answerOption)
        {
            _context.AnswerOptions.Add(answerOption);
            _context.SaveChanges();
        }

        public void CreateAnswerOptions(IEnumerable<AnswerOption> answerOptions)
        {
            _context.AnswerOptions.AddRange(answerOptions);
            _context.SaveChanges();
        }

        public void EditAnswerOption(AnswerOption answerOption)
        {
            _context.AnswerOptions.Update(answerOption);
            _context.SaveChanges();
        }

        public void EditAnswerOptions(IEnumerable<AnswerOption> answerOptions)
        {
            _context.AnswerOptions.UpdateRange(answerOptions);
            _context.SaveChanges();
        }

        public AnswerOption GetAnswerOption(long id)
        {
            try
            {
                return _context.AnswerOptions.Single(x => x.Id == id);
            }
            catch(InvalidOperationException)
            {
                throw new AnswerOptionNotFound(id);
            }
        }

        public IEnumerable<AnswerOption> GetAnswerOptionsForQuestion(long questionId)
        {
            return _context.AnswerOptions.Where(x => x.QuestionId == questionId).ToList();
        }
    }
}
