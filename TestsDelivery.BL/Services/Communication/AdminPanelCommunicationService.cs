﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestsDelivery.BL.Shared.Clients.Integration;
using TestsDelivery.UserModels.ScheduledTest;

namespace TestsDelivery.BL.Services.Communication
{
    public class AdminPanelCommunicationService : IAdminPanelCommunicationService
    {
        private readonly IIntegrationApiClient _apiClient;
        private readonly string _instanceUrl;
        private readonly IMapper _mapper;

        public AdminPanelCommunicationService(
            IIntegrationApiClient apiClient,
            string instanceUrl,
            IMapper mapper)
        {
            _apiClient = apiClient;
            _instanceUrl = instanceUrl;
            _mapper = mapper;
        }

        public async Task ScheduleTest(ScheduledTestDetailedModel test)
        {
            await _apiClient.PostAsync<ScheduledTestDetailedModel, IEnumerable<ScheduledTestInstanceReadModel>>($"{_instanceUrl}api/ScheduledTests", test);
        }
    }
}
