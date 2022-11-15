using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MyHangFireDemo.Helpers;
using MyHangFireDemo.Services;
using MyHangFireDemo.Utilitities;
using MyHangFireDemo.Utility;
using Amazon.S3;

namespace MyHangFireDemo
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
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyHangFireDemo", Version = "v1" });
            });

            // SQL Server Service Configuration
            services.AddDbContext<SQLDataContext>(Options => Options.UseSqlServer(Configuration.GetConnectionString("BookDBConnection")));

            // Hangfire Job Configuration - Start
            services.AddHangfire(x =>
            {
                x.UseSqlServerStorage(Configuration.GetConnectionString("HangfireDBConnection"));
            });
            services.AddHangfireServer();

            // Hangfire Job Service Configuration
            services.AddScoped<IJobService, JobService>();

            // XML Utility Configuration
            services.AddScoped<IXMLUtility, XMLUtility>();

            // SQL Book Utility Configuration
            services.AddScoped<ISQLBookUtility, SQLBookUtility>();

            // DynamoDB Book Utility Configuration
            services.AddScoped<IDynamoBookUtility, DynamoBookUtility>();
            services.AddScoped<IDynamoDBContext, DynamoDBContext>();

            //AWS Configuration
            var options = Configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(options);
            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonDynamoDB>();
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyHangFireDemo v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHangfireDashboard();
        }
    }
}
