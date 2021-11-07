using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Exceptions.Questions;
using TestsDelivery.DAL.Models.Questions;

namespace TestsDelivery.DAL.Repositories.Questions
{
    public class QuestionsRepository : IQuestionsRepository
    {
        private readonly TestsDeliveryContext _context;

        public QuestionsRepository(TestsDeliveryContext context, IMapper mapper)
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
                var existingQuestion = _context.Questions.Single(x => x.Id == question.Id);
                if (existingQuestion.ItemType != question.ItemType)
                    throw new QuestionIncorrectTypeException(question.ItemType, existingQuestion.ItemType);

                _context.Questions.Update(question);
                _context.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                throw new QuestionNotFoundException(question.Id);
            }
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
    }
}
