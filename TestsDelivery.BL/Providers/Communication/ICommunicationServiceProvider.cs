namespace TestsDelivery.BL.Providers.Communication
{
    public interface ICommunicationServiceProvider
    {
        TInterface Get<TInterface>(string instanceName);
    }
}
