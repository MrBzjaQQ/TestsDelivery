using System;
using AdminPanel.Logging;
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
using TestsDelivery.BL.Mediators;
using TestsDelivery.BL.Services;
using TestsDelivery.BL.Services.Subjects;
using TestsDelivery.BL.Services.Users;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Models.Identity;
using TestsDelivery.DAL.Repositories.Subjects;
using TestsDelivery.Options.Tokens;

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
                options => options.UseSqlServer(Configuration.GetConnectionString("TestsDeliveryConnection"),
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

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton(new IdentityErrorDescriber());
            services.AddSingleton(authOptions);

            services.AddScoped(typeof(IAppLogging<>), typeof(AppLogging<>));
            services.AddScoped<ISubjectsMediator, SubjectsMediator>();
            services.AddScoped<ISubjectsService, SubjectsService>();
            services.AddScoped<ISubjectsRepository, SubjectsRepository>();

            services.AddScoped<IUserService, UserService>();
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

            app.UseAuthentication();
            app.UseRouting();
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
