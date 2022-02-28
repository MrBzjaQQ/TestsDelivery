using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public void DeleteAnswerOptions(IEnumerable<long> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            var options = new List<AnswerOption>();
            foreach (var id in ids)
                options.Add(new AnswerOption { Id = id });
            
            _context.AttachRange(options);
            _context.RemoveRange(options);
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
            return _context.AnswerOptions.Where(x => x.QuestionId == questionId).AsNoTracking().ToList();
        }

        public IEnumerable<AnswerOption> GetAnswerOptionsForQuestionIds(IEnumerable<long> questionIds)
        {
            return _context.AnswerOptions.Where(x => questionIds.Contains(x.QuestionId)).AsNoTracking().ToList();
        }

        public void DeleteAnswerOptionsByQuestionId(long questionId)
        {
            var optionsToRemove = _context.AnswerOptions.Where(option => option.QuestionId == questionId);
            _context.RemoveRange(optionsToRemove);
        }
    }
}
