using System;
using TestsDelivery.Infrastructure.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
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
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Models.Identity;
using TestsDelivery.DAL.Repositories.AnswerOptions;
using TestsDelivery.DAL.Repositories.Candidate;
using TestsDelivery.DAL.Repositories.QuestionInTests;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.DAL.Repositories.ScheduledTest;
using TestsDelivery.DAL.Repositories.Subjects;
using TestsDelivery.DAL.Repositories.Test;
using TestsDelivery.Options.Tokens;
using TestsDelivery.BL.Providers.Communication;
using TestsDelivery.DAL.Repositories.CandidateInScheduledTest;
using TestsDelivery.BL.Shared.Factories;
using TestsDelivery.BL.Shared.Providers.Communication;
using TestsDelivery.BL.Shared.Clients.Integration;
using TestsDelivery.BL.Shared.Providers.Client;

namespace AdminPanel
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddDbContext<TestsDeliveryContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection"),
                optAction => optAction.MigrationsAssembly("TestsDelivery.DAL")));

            var authOptions = new AuthOptions(Configuration.GetValue<string>("TokenSecretKey"))
            {
                Issuer = Configuration.GetValue<string>("TokenIssuer"),
                Audience = Configuration.GetValue<string>("TokenAudience"),
                Lifetime = Configuration.GetValue<int>("TokenLifetime")
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
                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
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

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<TestsDeliveryContext>();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddHttpClient();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton(new IdentityErrorDescriber());
            services.AddSingleton(authOptions);

            // TODO: think about adding dependencies for each project
            services.AddScoped(typeof(IAppLogging<>), typeof(AppLogging<>));
            services.AddScoped<ISubjectsMediator, SubjectsMediator>();
            services.AddScoped<IScqMediator, ScqMediator>();
            services.AddScoped<IMcqMediator, McqMediator>();
            services.AddScoped<IEssayMediator, EssayMediator>();
            services.AddScoped<ICandidateMediator, CandidateMediator>();
            services.AddScoped<ITestMediator, TestMediator>();
            services.AddScoped<IScheduledTestMediator, ScheduledTestMediator>();

            services.AddScoped<ISubjectsService, SubjectsService>();
            services.AddScoped<IScqService, ScqService>();
            services.AddScoped<IMcqService, McqService>();
            services.AddScoped<IEssayService, EssayService>();
            services.AddScoped<ICandidateService, CandidateService>();
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IScheduledTestService, ScheduledTestService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITestPortalInstancesService, TestPortalInstancesService>();

            services.AddScoped<ISubjectsRepository, SubjectsRepository>();
            services.AddScoped<IQuestionsRepository, QuestionsRepository>();
            services.AddScoped<IAnswerOptionsRepository, AnswerOptionsRepository>();
            services.AddScoped<ICandidateRepository, CandidateRepository>();
            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<IQuestionInTestRepository, QuestionInTestRepository>();
            services.AddScoped<IScheduledTestRepository, ScheduledTestRepository>();
            services.AddScoped<IScheduledTestInstancesRepository, ScheduledTestInstancesRepository>();

            services.AddScoped<IScqModelValidator, ScqModelValidator>();
            services.AddScoped<IMcqModelValidator, McqModelValidator>();
            services.AddScoped<IEssayValidator, EssayValidator>();

            services.AddScoped<ICommunicationServiceFactory, CommunicationServiceFactory>();
            services.AddScoped<ICommunicationServiceProvider, CommunicationServiceProvider>();
            services.AddScoped<IIntegrationApiClient, IntegrationApiClient>();
            services.AddScoped<IHttpClientProvider, HttpClientProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                }
            });
        }
    }
}
