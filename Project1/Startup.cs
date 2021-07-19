using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.CodeGeneration.TypeScript;
using Project1.IRepositories;
using Project1.IServices;
using Project1.Models.MainContext;
using Project1.Repositories;
using Project1.Services;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Project1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IStepService, StepService>();

            MainDbContext.connectionString = Configuration.GetConnectionString("MainConnectionString");
            services.AddEntityFrameworkSqlServer().AddDbContext<MainDbContext>(options =>
            {
                options.UseLazyLoadingProxies(true)
                .UseSqlServer(Configuration.GetConnectionString("MainConnectionString"), serverDbContextOptionsBuilder =>
                {
                    int minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                    serverDbContextOptionsBuilder.CommandTimeout(minutes);
                    serverDbContextOptionsBuilder.EnableRetryOnFailure();
                });
            });

            services.AddMvc(
                            // for  Global Authorization
                            o =>
                            {
                                o.EnableEndpointRouting = false;
                            }
                            ).SetCompatibilityVersion(CompatibilityVersion.Latest).AddNewtonsoftJson(options =>
                            {
                                options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
                                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                            }).AddNewtonsoftJson();
            services.AddSwaggerGen(action =>
            {

                action.MapType<object>(() => new Microsoft.OpenApi.Models.OpenApiSchema { Type = "any" });
                action.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Masar WebApi", Version = "v1" });
                action.EnableAnnotations();


            });
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }


            app.UseRouting();

            app.UseMvc();


            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
            SeedData.Initialize(app.ApplicationServices);
            Task.Run(async () =>
            {
                string SourceDocumentAbsoluteUrl = Configuration["SwaggerToTypeScriptClientGeneratorSettings:SourceDocumentAbsoluteUrl"];
                string OutputDocumentRelativePath = Configuration["SwaggerToTypeScriptClientGeneratorSettings:OutputDocumentRelativePath"];
                var document = await OpenApiDocument.FromUrlAsync(SourceDocumentAbsoluteUrl);
                var settings = new TypeScriptClientGeneratorSettings()
                {
                    OperationNameGenerator = new SwaggerClientOperationNameGenerator(),
                    ClassName = "SwaggerClient",
                    Template = TypeScriptTemplate.Angular,
                    RxJsVersion = 6.0M,
                    HttpClass = HttpClass.HttpClient,
                    InjectionTokenType = InjectionTokenType.InjectionToken,
                    BaseUrlTokenName = "API_BASE_URL",
                    WrapDtoExceptions = true
                };
                var generator = new TypeScriptClientGenerator(document, settings);
                var code = generator.GenerateFile();
                new FileInfo(OutputDocumentRelativePath).Directory.Create();
                try
                {
                    File.WriteAllText(OutputDocumentRelativePath, code);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }
    }
}
