namespace TestsDelivery.BL.Shared.Providers.Communication
{
    public interface ICommunicationServiceProvider
    {
        TInterface Get<TInterface>(string instanceName);
    }
}
