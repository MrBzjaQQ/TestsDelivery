using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using TestsDelivery.Infrastructure.Logging;
using TestsPortal.BL.Mediators.ScheduledTests;
using TestsPortal.BL.Services.Candidates;
using TestsPortal.BL.Services.EmailServices;
using TestsPortal.BL.Services.Questions;
using TestsPortal.BL.Services.ScheduledTests;
using TestsPortal.BL.Services.Subjects;
using TestsPortal.BL.Services.Tests;
using TestsPortal.DAL.Data;
using TestsPortal.DAL.Repositories.AnswerOptions;
using TestsPortal.DAL.Repositories.Candidate;
using TestsPortal.DAL.Repositories.Questions;
using TestsPortal.DAL.Repositories.ScheduledTests;
using TestsPortal.DAL.Repositories.Subjects;
using TestsPortal.DAL.Repositories.Tests;

namespace TestsPortal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<TestsPortalContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("TestsPortalConnection"),
                optAction => optAction.MigrationsAssembly("TestsPortal.DAL")));

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped(typeof(IAppLogging<>), typeof(AppLogging<>));

            services.AddScoped<IScheduledTestMediator, ScheduledTestMediator>();

            services.AddScoped<ITestsService, TestsService>();
            services.AddScoped<ICandidateService, CandidateService>();
            services.AddScoped<IQuestionsService, QuestionsService>();
            services.AddScoped<IScheduledTestService, ScheduledTestService>();
            services.AddScoped<ISubjectsService, SubjectsService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICandidateNotificationService, CandidateNotificationService>();

            services.AddScoped<IAnswerOptionsRepository, AnswerOptionsRepository>();
            services.AddScoped<ICandidatesRepository, CandidatesRepository>();
            services.AddScoped<IQuestionsRepository, QuestionsRepository>();
            services.AddScoped<IScheduledTestsRepository, ScheduledTestsRepository>();
            services.AddScoped<ISubjectsRepository, SubjectsRepository>();
            services.AddScoped<ITestsRepository, TestsRepository>();
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
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
