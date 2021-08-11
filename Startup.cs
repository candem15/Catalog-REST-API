using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using catalog.Repositories;
using MongoDB.Driver;
using catalog.Settings;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace catalog
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
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String)); //Guid type representation with not familiar form in MongoDb so we use "serializer" to cover that.
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String)); //DateTimeOffset type representation with not familiar form in MongoDb so we use "serializer" to cover that.
            var mongoDbSettings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>(); //For settings of client recieved from "Settings/MongoDbSettings.cs" and use "Configuration" of "Startup" that haven't populated yet. For using mongoDbSettings in other options so we extracted from AddSingleton service.

            services.AddSingleton<IMongoClient>(ServiceProvider=> //After setting ConnectionString MongoDb we need to register IMongoClient that we injected into MongoDBItemsRepository. Injecting dependency besides like InMemItemsRepository we have to construct explicitly because of additional configuration needed.
            {
                return new MongoClient(mongoDbSettings.ConnectionString); //IMongoClient instance construct.
            });
           
            services.AddSingleton<IItemsRepository,MongoDbItemsRepository>(); //If we need one instance entire lifetime and use it over and over again whenever it is needed, we are register service(dependency) as "Singleton" type to do that.

            services.AddControllers(Options=>
            {
                Options.SuppressAsyncSuffixInActionNames = false; //This option prevents to .NetCore removes async suffix from methods in runtime. If this is not setted we cant POST(create) item because at CreateItemAsync method in ItemsController "nameof" option cant find "GetItemAsync" because of that is turned in runtime to "GetItem".
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "catalog", Version = "v1" });
            });
            services.AddHealthChecks()
                .AddMongoDb(
                    mongoDbSettings.ConnectionString, 
                    name: "mongodb", 
                    timeout: TimeSpan.FromSeconds(3),
                    tags: new[]{"ready"}); //That adds HealthCheck service. Now we need to add middleware for it. Added healthcheck option for mongodb with name given "mongo", 3 second timeout and tags "ready".
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "catalog v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/health/ready",new HealthCheckOptions{  //This pick route that we use to health check endpoint. This works fine but cannot know dependency like MongoDb is healthy or not. Because of that we add nuget package for specificly MongoDB health check =>"dotnet add package AspNetCore.HealthChecks.MongoDb". 
                    Predicate = (check) => check.Tags.Contains("ready"), //Predicate is the way to filter which health checks we wanna include in this endpoint.
                    ResponseWriter = async(context, report) => //This used for get more specified response at healthchecks not just like "Healthy or Unhealthy".
                    {
                        var result = JsonSerializer.Serialize(
                            new{
                                status = report.Status.ToString(), //Healthy or Unhealthy
                                checks = report.Entries.Select(entry => new {
                                    name = entry.Key, //Dependency name. In this case "mongodb".
                                    status = entry.Value.Status.ToString(), //Healthy or Unhealthy again but for this dependency. 
                                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none", //Exception message if there is any.
                                    duration = entry.Value.Duration.ToString() //"Health check" response time.
                                })
                            }
                        );
                        context.Response.ContentType = MediaTypeNames.Application.Json; //This helps to return response nice JSON string format in Postman.
                        await context.Response.WriteAsync(result);
                    }
                }); //This pick route that we use to health check endpoint. This works fine but cannot know dependency like MongoDb is healthy or not. Because of that we add nuget package for specificly MongoDB health check =>"dotnet add package AspNetCore.HealthChecks.MongoDb". 
                
                endpoints.MapHealthChecks("/health/live",new HealthCheckOptions{
                    Predicate = (_) => false //In this case we saying false. By doing that, we are excluding every single health check and so that it would come back to as is service alive.
                });
            });
        }
    }
}
