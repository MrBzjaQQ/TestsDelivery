using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestsDelivery.BL.Shared.Clients.Integration;
using TestsDelivery.BL.Shared.Factories;
using TestsDelivery.BL.Shared.Providers.Client;
using TestsDelivery.Infrastructure.Logging;
using TestsPortal.BL.Factories.Communication;
using TestsPortal.BL.Mediators.Questions;
using TestsPortal.BL.Mediators.ScheduledTests;
using TestsPortal.BL.Mediators.TestProcesses;
using TestsPortal.BL.Services.AdminPanelInstances;
using TestsPortal.BL.Services.Candidates;
using TestsPortal.BL.Services.Communication;
using TestsPortal.BL.Services.EmailServices;
using TestsPortal.BL.Services.Questions;
using TestsPortal.BL.Services.Questions.TypedQuestion;
using TestsPortal.BL.Services.ScheduledTests;
using TestsPortal.BL.Services.Subjects;
using TestsPortal.BL.Services.TestProcesses;
using TestsPortal.BL.Services.Tests;
using TestsPortal.DAL.Repositories.AnswerOptions;
using TestsPortal.DAL.Repositories.Answers;
using TestsPortal.DAL.Repositories.Candidate;
using TestsPortal.DAL.Repositories.Questions;
using TestsPortal.DAL.Repositories.ScheduledTestInstances;
using TestsPortal.DAL.Repositories.ScheduledTests;
using TestsPortal.DAL.Repositories.Subjects;
using TestsPortal.DAL.Repositories.Tests;

namespace TestsPortal
{
    public static class Dependencies
    {
        public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();

            services.AddScoped(typeof(IAppLogging<>), typeof(AppLogging<>));

            services.AddScoped<IScheduledTestMediator, ScheduledTestMediator>();
            services.AddScoped<ITestProcessMediator, TestProcessMediator>();
            services.AddScoped<IEssayMediator, EssayMediator>();
            services.AddScoped<IMultipleChoiceService, MultipleChoiceService>();
            services.AddScoped<ISingleChoiceService, SingleChoiceService>();

            services.AddScoped<ITestsService, TestsService>();
            services.AddScoped<ICandidateService, CandidateService>();
            services.AddScoped<IQuestionsService, QuestionsService>();
            services.AddScoped<IScheduledTestService, ScheduledTestService>();
            services.AddScoped<ISubjectsService, SubjectsService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICandidateNotificationService, CandidateNotificationService>();
            services.AddScoped<ITestProcessService, TestProcessService>();
            services.AddScoped<IEssayService, EssayService>();
            services.AddScoped<IMultipleChoiceService, MultipleChoiceService>();
            services.AddScoped<ISingleChoiceService, SingleChoiceService>();
            services.AddScoped<ITestsPortalCommunicationService, TestsPortalCommunicationService>();
            services.AddScoped<IAdminPanelInstancesService, AdminPanelInstancesService>();

            services.AddScoped<IAnswerOptionsRepository, AnswerOptionsRepository>();
            services.AddScoped<ICandidatesRepository, CandidatesRepository>();
            services.AddScoped<IQuestionsRepository, QuestionsRepository>();
            services.AddScoped<IScheduledTestsRepository, ScheduledTestsRepository>();
            services.AddScoped<ISubjectsRepository, SubjectsRepository>();
            services.AddScoped<IScheduledTestInstancesRepository, ScheduledTestInstancesRepository>();
            services.AddScoped<ITestsRepository, TestsRepository>();
            services.AddScoped<ITextAnswersRepository, TextAnswersRepository>();
            services.AddScoped<IChoiceAnswersRepository, ChoiceAnswersRepository>();

            services.AddScoped<IIntegrationApiClient, IntegrationApiClient>();
            services.AddScoped<ICommunicationServiceFactory, CommunicationServiceFactory>();
            services.AddScoped<IHttpClientProvider, HttpClientProvider>();
        }
    }
}
