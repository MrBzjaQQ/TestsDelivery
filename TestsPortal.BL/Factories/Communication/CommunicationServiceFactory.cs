using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsDelivery.BL.Shared.Clients.Integration;
using TestsDelivery.BL.Shared.Factories;
using TestsPortal.BL.Services.Communication;

namespace TestsPortal.BL.Factories.Communication
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
                nameof(ITestsPortalCommunicationService) => typeof(TestsPortalCommunicationService),
                _ => throw new NotImplementedException($"Interface {typeof(TInterface).Name} is not implemented.")
            };
        }
    }
}
