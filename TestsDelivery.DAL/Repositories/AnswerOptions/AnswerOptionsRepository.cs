using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Exceptions.AnswerOptions;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.AnswerOptions
{
    public class AnswerOptionsRepository : BaseRepository<TestsDeliveryContext, AnswerOption>, IAnswerOptionsRepository
    {
        public AnswerOptionsRepository(TestsDeliveryContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public void CreateAnswerOption(AnswerOption answerOption)
        {
            Context.AnswerOptions.Add(answerOption);
            Context.SaveChanges();
        }

        public void CreateAnswerOptions(IEnumerable<AnswerOption> answerOptions)
        {
            Context.AnswerOptions.AddRange(answerOptions);
            Context.SaveChanges();
        }

        public void EditAnswerOption(AnswerOption answerOption)
        {
            Context.AnswerOptions.Update(answerOption);
            Context.SaveChanges();
        }

        public void EditAnswerOptions(IEnumerable<AnswerOption> answerOptions)
        {
            Context.AnswerOptions.UpdateRange(answerOptions);
            Context.SaveChanges();
        }

        public void DeleteAnswerOptions(IEnumerable<long> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            var options = new List<AnswerOption>();
            foreach (var id in ids)
                options.Add(new AnswerOption { Id = id });

            Context.AttachRange(options);
            Context.RemoveRange(options);
            Context.SaveChanges();
        }

        public AnswerOption GetAnswerOption(long id)
        {
            try
            {
                return Context.AnswerOptions.Single(x => x.Id == id);
            }
            catch(InvalidOperationException)
            {
                throw new AnswerOptionNotFound(id);
            }
        }

        public IEnumerable<AnswerOption> GetAnswerOptionsForQuestion(long questionId)
        {
            return Context.AnswerOptions.Where(x => x.QuestionId == questionId).AsNoTracking().ToList();
        }

        public IEnumerable<AnswerOption> GetAnswerOptionsForQuestionIds(IEnumerable<long> questionIds)
        {
            return Context.AnswerOptions.Where(x => questionIds.Contains(x.QuestionId)).AsNoTracking().ToList();
        }

        public void DeleteAnswerOptionsByQuestionId(long questionId)
        {
            var optionsToRemove = Context.AnswerOptions.Where(option => option.QuestionId == questionId);
            Context.RemoveRange(optionsToRemove);
        }
    }
}
