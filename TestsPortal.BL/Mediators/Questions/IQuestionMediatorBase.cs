namespace TestsPortal.BL.Mediators.Questions
{
    public interface IQuestionMediatorBase<TAnswerModel>
    {
        void PostAnswer(TAnswerModel answer);
    }
}
