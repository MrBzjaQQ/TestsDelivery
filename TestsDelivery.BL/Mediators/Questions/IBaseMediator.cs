namespace TestsDelivery.BL.Mediators.Questions
{
    public interface IBaseMediator<in TCreateModel, in TEditModel, out TReadModel>
    {
        TReadModel CreateQuestion(TCreateModel model);

        void EditQuestion(TEditModel model);

        TReadModel GetQuestion(long id);
    }
}
