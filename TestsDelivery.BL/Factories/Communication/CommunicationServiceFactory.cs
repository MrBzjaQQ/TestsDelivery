using AutoMapper;
using System;
using TestsDelivery.BL.Shared.Clients.Integration;
using TestsDelivery.BL.Services.Communication;
using TestsDelivery.BL.Shared.Factories;

namespace TestsDelivery.BL.Factories
{
    public class CommunicationServiceFactory : ICommunicationServiceFactory
    {
        private readonly IMapper _mapper;

        public CommunicationServiceFactory(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TInterface Create<TInterface>(IIntegrationApiClient apiClient, string instanceUrl)
        {
            var implementationType = GetImplementationType<TInterface>();
            return (TInterface)Activator.CreateInstance(implementationType, apiClient, instanceUrl, _mapper);
        }

        private Type GetImplementationType<TInterface>()
        {
            return typeof(TInterface).Name switch
            {
                nameof(IAdminPanelCommunicationService) => typeof(AdminPanelCommunicationService),
                _ => throw new NotImplementedException($"Interface {typeof(TInterface).Name} is not implemented.")
            };
        }
    }
}
