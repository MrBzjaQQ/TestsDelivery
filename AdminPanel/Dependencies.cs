using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using TestsDelivery.BL.Factories;
using TestsDelivery.BL.Mediators.Candidate;
using TestsDelivery.BL.Mediators.Questions.Essay;
using TestsDelivery.BL.Mediators.Questions.MultipleChoice;
using TestsDelivery.BL.Mediators.Questions.SingleChoice;
using TestsDelivery.BL.Mediators.ScheduledTest;
using TestsDelivery.BL.Mediators.Subjects;
using TestsDelivery.BL.Mediators.Test;
using TestsDelivery.BL.Services.Candidates;
using TestsDelivery.BL.Services.Questions.Essay;
using TestsDelivery.BL.Services.Questions.MultipleChoice;
using TestsDelivery.BL.Services.Questions.SingleChoice;
using TestsDelivery.BL.Services.ScheduledTest;
using TestsDelivery.BL.Services.Subjects;
using TestsDelivery.BL.Services.Test;
using TestsDelivery.BL.Services.TestPortalInstances;
using TestsDelivery.BL.Services.Users;
using TestsDelivery.UserModels.Validators.Questions;
using TestsDelivery.DAL.Repositories.AnswerOptions;
using TestsDelivery.DAL.Repositories.Candidate;
using TestsDelivery.DAL.Repositories.QuestionInTests;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.DAL.Repositories.ScheduledTest;
using TestsDelivery.DAL.Repositories.Subjects;
using TestsDelivery.DAL.Repositories.Test;
using TestsDelivery.BL.Providers.Communication;
using TestsDelivery.DAL.Repositories.CandidateInScheduledTest;
using TestsDelivery.BL.Shared.Factories;
using TestsDelivery.BL.Shared.Providers.Communication;
using TestsDelivery.BL.Shared.Clients.Integration;
using TestsDelivery.BL.Shared.Providers.Client;
using TestsDelivery.BL.Mediators.TestProcesses;
using TestsDelivery.BL.Services.TestProcesses;
using TestsDelivery.BL.Mediators.Questions.Lists;
using TestsDelivery.BL.Services.Questions;
using TestsDelivery.BL.Mediators.Marking.Essay;
using TestsDelivery.BL.Mediators.Marking.MultipleChoice;
using TestsDelivery.BL.Mediators.Marking.SingleChoice;
using TestsDelivery.BL.Services.Marking.Essay;
using TestsDelivery.BL.Services.Marking.MultipleChoice;
using TestsDelivery.BL.Services.Marking.SingleChoice;
using TestsDelivery.DAL.Repositories.Marking.Choice;
using TestsDelivery.DAL.Repositories.Marking.Essay;
using TestsDelivery.DAL.Repositories.Answers;
using TestsDelivery.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TestsDelivery.Options.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace AdminPanel
{
    public static class Dependencies
    {
        public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var authOptions = new AuthOptions(configuration.GetValue<string>("IssuerSigningKey"))
            {
                Issuer = configuration.GetValue<string>("TokenIssuer"),
                Audience = configuration.GetValue<string>("TokenAudience"),
                Lifetime = configuration.GetValue<int>("TokenLifetime"),
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = authOptions.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = authOptions.GetIssuerSigningKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddHttpClient();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton(new IdentityErrorDescriber());
            services.AddSingleton(authOptions);

            services.AddScoped(typeof(IAppLogging<>), typeof(AppLogging<>));
            services.AddScoped<ISubjectsMediator, SubjectsMediator>();
            services.AddScoped<IScqMediator, ScqMediator>();
            services.AddScoped<IMcqMediator, McqMediator>();
            services.AddScoped<IEssayMediator, EssayMediator>();
            services.AddScoped<ICandidateMediator, CandidateMediator>();
            services.AddScoped<ITestMediator, TestMediator>();
            services.AddScoped<IScheduledTestMediator, ScheduledTestMediator>();
            services.AddScoped<ITestProcessMediator, TestProcessMediator>();
            services.AddScoped<IQuestionListsMediator, QuestionListsMediator>();
            services.AddScoped<IEssayMarkMediator, EssayMarkMediator>();
            services.AddScoped<IMultipleChoiceMarkMediator, MultipleChoiceMarkMediator>();
            services.AddScoped<ISingleChoiceMarkMediator, SingleChoiceMarkMediator>();

            services.AddScoped<ISubjectsService, SubjectsService>();
            services.AddScoped<IScqService, ScqService>();
            services.AddScoped<IMcqService, McqService>();
            services.AddScoped<IEssayService, EssayService>();
            services.AddScoped<ICandidateService, CandidateService>();
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IScheduledTestService, ScheduledTestService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITestPortalInstancesService, TestPortalInstancesService>();
            services.AddScoped<ITestProcessService, TestProcessService>();
            services.AddScoped<IQuestionListsService, QuestionListsService>();
            services.AddScoped<IEssayMarkService, EssayMarkService>();
            services.AddScoped<IMultipleChoiceMarkService, MultipleChoiceMarkService>();
            services.AddScoped<ISingleChoiceMarkService, SingleChoiceMarkService>();

            services.AddScoped<ISubjectsRepository, SubjectsRepository>();
            services.AddScoped<IQuestionsRepository, QuestionsRepository>();
            services.AddScoped<IAnswerOptionsRepository, AnswerOptionsRepository>();
            services.AddScoped<ICandidateRepository, CandidateRepository>();
            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<IQuestionInTestRepository, QuestionInTestRepository>();
            services.AddScoped<IScheduledTestRepository, ScheduledTestRepository>();
            services.AddScoped<IScheduledTestInstancesRepository, ScheduledTestInstancesRepository>();
            services.AddScoped<IChoiceMarkingRepository, ChoiceMarkingRepository>();
            services.AddScoped<IEssayMarkingRepository, EssayMarkingRepository>();
            services.AddScoped<IChoiceAnswersRepository, ChoiceAnswersRepository>();
            services.AddScoped<ITextAnswersRepository, TextAnswersRepository>();

            services.AddScoped<IScqModelValidator, ScqModelValidator>();
            services.AddScoped<IMcqModelValidator, McqModelValidator>();
            services.AddScoped<IEssayValidator, EssayValidator>();

            services.AddScoped<ICommunicationServiceFactory, CommunicationServiceFactory>();
            services.AddScoped<ICommunicationServiceProvider, CommunicationServiceProvider>();
            services.AddScoped<IIntegrationApiClient, IntegrationApiClient>();
            services.AddScoped<IHttpClientProvider, HttpClientProvider>();
        }
    }
}
