﻿using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Services.Questions
{
    public interface IBaseQuestionService<TQuestionBase>
    {
        TQuestionBase CreateQuestion(TQuestionBase question);

        void EditQuestion(TQuestionBase question);

        TQuestionBase GetQuestion(long id);

        void DeleteQuestion(long id);
    }
}
