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

            services.AddSingleton<IMongoClient>(ServiceProvider=> //After setting ConnectionString MongoDb we need to register IMongoClient that we injected into MongoDBItemsRepository. Injecting dependency besides like InMemItemsRepository we have to construct explicitly because of additional configuration needed.
            {
                var settings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>(); //For settings of client recieved from "Settings/MongoDbSettings.cs" and use "Configuration" of "Startup" that haven't populated yet.
                return new MongoClient(settings.ConnectionString); //IMongoClient instance construct.
            });
           
            services.AddSingleton<IItemsRepository,MongoDbItemsRepository>(); //If we need one instance entire lifetime and use it over and over again whenever it is needed, we are register service(dependency) as "Singleton" type to do that.

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "catalog", Version = "v1" });
            });
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
            });
        }
    }
}
