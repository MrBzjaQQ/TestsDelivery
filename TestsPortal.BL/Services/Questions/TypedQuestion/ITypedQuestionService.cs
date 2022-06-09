namespace TestsPortal.BL.Services.Questions.TypedQuestion
{
    public interface ITypedQuestionService<TDomainAnswer>
    {
        void PostAnswer(TDomainAnswer answer);
    }
}
